using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.Text;
using System.Text.Json;
namespace ProjectManagement.Service.Service.Requests
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IGenericRepository<RequestStatus> requestStatusRepository;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> requestRepository;
        private readonly IConfiguration configuration;
        public RequestStatusService(
            IGenericRepository<RequestStatus> requestStatusRepository,
            IGenericRepository<Domain.Entities.Requests.Request> requestRepository,
            IConfiguration configuration)
        {
            this.requestStatusRepository = requestStatusRepository;
            this.requestRepository = requestRepository;
            this.configuration = configuration;
        }

        public async ValueTask<List<RequestStatusModel>> GetAsync()
        {
            var status = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var model = status.Select(x => new RequestStatusModel().MapFromEntity(x)).ToList();
            return model;
        }

        public async ValueTask<bool> CreateAsync(RequestStatusForCreateDTO dto)
        {
            var requestStatus = new RequestStatus
            {
                Title = dto.Title,
                CreatedAt = DateTime.UtcNow
            };

            await requestStatusRepository.CreateAsync(requestStatus);
            await requestStatusRepository.SaveChangesAsync();
            return true;
        }


        public async ValueTask<bool> DeleteAsync(int id)
        {
            var existStatus = await requestStatusRepository.GetAsync(x => x.Id == id);

            if (existStatus is null) throw new ProjectManagementException(404, "request_status_not_found");


            existStatus.IsDeleted = 1;

            requestStatusRepository.UpdateAsync(existStatus);
            await requestStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> UpdateAsync(int id, RequestStatusForCreateDTO dto)
        {
            var existStatus = await requestStatusRepository.GetAsync(x => x.Id == id);

            if (existStatus is null) throw new ProjectManagementException(404, "request_status_not_found");


            existStatus.Title = dto.Title;

            requestStatusRepository.UpdateAsync(existStatus);
            await requestStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<PagedResult<RequestModel>> GetRequeststAsync(RequestForFilterDTO dto)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            using (var db = new NpgsqlConnection(connectionString))
            {
                var sql = new StringBuilder(@"
                    SELECT 
                        r.*, 
                        rs.""Id"" AS RequestStatus_Id, 
                        rs.""Title"",
                        rs.""Id""
                    FROM ""Requests"" r
                    LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                    WHERE 1=1
                    ");

                sql.Append(" AND r.\"IsDeleted\" = 0");
                sql.Append(" ORDER BY r.\"CreatedAt\" DESC");

                var countSql = new StringBuilder("SELECT COUNT(*) FROM \"Requests\" WHERE 1=1");

                var parameters = new DynamicParameters();

                void AppendFilters(StringBuilder sql, StringBuilder countSql, DynamicParameters parameters, string columnName, List<string>? values, bool strict = false)
                {
                    if (values != null && values.Any())
                    {
                        var paramNames = new List<string>();
                        for (int i = 0; i < values.Count; i++)
                        {
                            string paramName = $"@{columnName}{i}";
                            paramNames.Add(paramName);
                            parameters.Add(paramName, strict ? values[i] : $"%{values[i]}%");
                        }

                        string condition = string.Join(" OR ", paramNames.Select(p => strict
                            ? $"\"{columnName}\" = {p}"
                            : $"\"{columnName}\" ILIKE {p}"));

                        sql.Append($" AND ({condition})");
                        countSql.Append($" AND ({condition})");
                    }
                }

                AppendFilters(sql, countSql, parameters, "InquiryType", dto.InquiryType);
                AppendFilters(sql, countSql, parameters, "InquiryField", dto.InquiryField);
                AppendFilters(sql, countSql, parameters, "CompanyName", dto.CompanyName);
                AppendFilters(sql, countSql, parameters, "Department", dto.Department);
                AppendFilters(sql, countSql, parameters, "ResponsiblePerson", dto.ResponsiblePerson);
                AppendFilters(sql, countSql, parameters, "ClientCompany", dto.ClientCompany, strict: true);
                AppendFilters(sql, countSql, parameters, "Email", dto.Email);
                AppendFilters(sql, countSql, parameters, "ProcessingStatus", dto.ProcessingStatus);
                AppendFilters(sql, countSql, parameters, "FinalResult", dto.FinalResult);
                AppendFilters(sql, countSql, parameters, "Notes", dto.Notes);
                AppendFilters(sql, countSql, parameters, "Date", dto.Date);
                AppendFilters(sql, countSql, parameters, "ContactNumber", dto.ContactNumber);
                AppendFilters(sql, countSql, parameters, "ProjectDetails", dto.ProjectDetails);

                if (dto?.RequestStatusId != null)
                {
                    sql.Append(" AND \"RequestStatusId\" = @RequestStatusId");
                    countSql.Append(" AND \"RequestStatusId\" = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.RequestStatusId);
                }

                if (dto?.CreatedAt != null)
                {
                    sql.Append(" AND \"CreatedAt\" >= @CreatedAt");
                    countSql.Append(" AND \"CreatedAt\" >= @CreatedAt");
                    parameters.Add("@CreatedAt", dto.CreatedAt);
                }

                int totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);

                if (totalCount == 0)
                {
                    return PagedResult<RequestModel>.Create(new List<RequestModel>(), 0, dto.PageSize, 0, dto.PageIndex, 0);
                }

                if (!string.IsNullOrEmpty(dto.SortBy) && !string.IsNullOrEmpty(dto.Order))
                {
                    sql.Append($" ORDER BY \"{dto.SortBy}\" {(dto.Order.ToLower() == "ascend" ? "ASC" : "DESC")}");
                }

                int skip = (dto.PageIndex - 1) * dto.PageSize;
                sql.Append(" LIMIT @PageSize OFFSET @Offset;");
                parameters.Add("@PageSize", dto.PageSize);
                parameters.Add("@Offset", skip);

                var list = await db.QueryAsync<RequestModel, RequestStatusModel, RequestModel>(
                     sql.ToString(),
                     (request, status) =>
                     {
                         request.RequestStatus = status;
                         return request;
                     },
                     parameters,
                     splitOn: "RequestStatus_Id"
                    );


                int totalPages = (int)Math.Ceiling((double)totalCount / dto.PageSize);

                return PagedResult<RequestModel>.Create(list.ToList(), totalCount, dto.PageSize, list.Count(), dto.PageIndex, totalPages);
            }
        }


        public async ValueTask<string> CreateRequestAsync(int RequestStatusId)
        {
            var jsonFilePath = Path.Combine("wwwroot", "images", $"request.json");

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("Файл JSON не найден.");
                return "SAS";
            }


            string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            List<RequestForCreateDTO>? requests = JsonSerializer.Deserialize<List<RequestForCreateDTO>>(jsonContent, options);

            if (requests == null || requests.Count == 0)
            {
                Console.WriteLine("JSON-файл пуст или содержит некорректные данные.");
                return "AS";
            }

            foreach (var item in requests)
            {
                var request = new Domain.Entities.Requests.Request
                {
                    Client = item.Client,
                    ClientCompany = item.ClientCompany,
                    CompanyName = item.CompanyName,
                    ContactNumber = item.ContactNumber,
                    Department = item.Department,
                    Email = item.Email,
                    FinalResult = item.FinalResult,
                    InquiryField = item.InquiryField,
                    InquiryType = item.InquiryType,
                    ProcessingStatus = item.ResponseStatus,
                    Notes = item.Notes,
                    ProjectDetails = item.ProjectDescription,
                    ResponsiblePerson = item.ResponsiblePerson,
                    CreatedAt = DateTime.UtcNow,
                    RequestStatusId = item.RequestStatusId,
                    Date = item.Date,
                };

                await requestRepository.CreateAsync(request);
            }

            await requestRepository.SaveChangesAsync();

            return "success";
        }


        public async ValueTask<bool> CreateRequest(RequestForCreateDTO dto)
        {
            var request = new Domain.Entities.Requests.Request
            {
                Client = dto.Client,
                ClientCompany = dto.ClientCompany,
                CompanyName = dto.CompanyName,
                ContactNumber = dto.ContactNumber,
                Department = dto.Department,
                Email = dto.Email,
                FinalResult = dto.FinalResult,
                InquiryField = dto.InquiryField,
                InquiryType = dto.InquiryType,
                ProcessingStatus = dto.ProcessingStatus,
                Notes = dto.Notes,
                ProjectDetails = dto.ProjectDetails,
                ResponsiblePerson = dto.ResponsiblePerson,
                CreatedAt = DateTime.UtcNow,
                RequestStatusId = dto.RequestStatusId,
                Date = dto.Date,
                Deadline = dto.Deadline,
                Status = dto.Status,
                Priority = dto.Priority,
            };

            await requestRepository.CreateAsync(request);

            await requestRepository.SaveChangesAsync();

            return true;
        }


        public async ValueTask<bool> UpdateRequest(int id, RequestForCreateDTO dto)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 0).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            existRequest.Client = dto.Client;
            existRequest.ClientCompany = dto.ClientCompany;
            existRequest.CompanyName = dto.CompanyName;
            existRequest.ContactNumber = dto.ContactNumber;
            existRequest.Department = dto.Department;
            existRequest.Email = dto.Email;
            existRequest.FinalResult = dto.FinalResult;
            existRequest.InquiryField = dto.InquiryField;
            existRequest.InquiryType = dto.InquiryType;
            existRequest.ProcessingStatus = dto.ProcessingStatus;
            existRequest.Notes = dto.Notes;
            existRequest.ProjectDetails = dto.ProjectDetails;
            existRequest.ResponsiblePerson = dto.ResponsiblePerson;
            existRequest.Date = dto.Date;
            existRequest.Status = dto.Status;
            existRequest.Priority = dto.Priority;
            existRequest.Deadline = dto.Deadline;

            existRequest.RequestStatusId = dto.RequestStatusId;
            existRequest.UpdatedAt = DateTime.UtcNow;
            existRequest.CreatedAt = existRequest.CreatedAt ?? DateTime.UtcNow;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();

            return true;
        }

        public async ValueTask<bool> DeleteRequest(int id)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 0).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            existRequest.IsDeleted = 1;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> RecoverRequest(int id)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 1).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            existRequest.IsDeleted = 0;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<List<RequestFilterModel>> GetFilterValue()
        {
            var existRequest = await requestRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();
            var emptyValue = "Unknown";

            var groupedFilters = new List<RequestFilterModel>();

            var fields = new Dictionary<string, Func<Domain.Entities.Requests.Request, string>>
            {
                { "Client", x => x.Client },
                { "ClientCompany", x => x.ClientCompany },
                { "CompanyName", x => x.CompanyName },
                { "ContactNumber", x => x.ContactNumber },
                { "Notes", x => x.Notes },
                { "ProjectDetails", x => x.ProjectDetails },
                { "Date", x => x.Date?.ToString() },
                { "Department", x => x.Department },
                { "Email", x => x.Email },
                { "InquiryField", x => x.InquiryField },
                { "InquiryType", x => x.InquiryType },
                { "FinalResult", x => x.FinalResult },
                { "ProcessingStatus", x => x.ProcessingStatus },
                { "ResponsiblePerson", x => x.ResponsiblePerson },
            };

            foreach (var field in fields)
            {
                var uniqueValues = existRequest
                    .Select(x => field.Value(x) ?? emptyValue)
                    .Distinct()
                    .Select(value => new RequestFilterModel
                    {
                        Text = value,
                        Value = field.Key
                    })
                    .ToList();

                groupedFilters.AddRange(uniqueValues);
            }

            return groupedFilters;
        }
    }
}
