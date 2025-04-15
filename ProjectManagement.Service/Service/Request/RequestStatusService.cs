using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using TrustyTalents.Service.Services.Emails;
namespace ProjectManagement.Service.Service.Requests
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IGenericRepository<RequestStatus> requestStatusRepository;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> requestRepository;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGenericRepository<Logs> _logRepository;
        private readonly IEmailInboxService emailInboxService;
        public RequestStatusService(
            IGenericRepository<RequestStatus> requestStatusRepository,
            IGenericRepository<Domain.Entities.Requests.Request> requestRepository,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<Logs> logRepository,
            IEmailInboxService emailInboxService
           )
        {
            this.requestStatusRepository = requestStatusRepository;
            this.requestRepository = requestRepository;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logRepository = logRepository;
            this.emailInboxService = emailInboxService;
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

            var requestsCount = await requestRepository.GetAll(x => x.IsDeleted == 0 && x.RequestStatusId == existStatus.Id).ToListAsync();

            existStatus.IsDeleted = 0;

            foreach(var item in requestsCount)
            {
                item.IsDeleted = 0;
                requestRepository.UpdateAsync(item);
            }

            requestStatusRepository.UpdateAsync(existStatus);
            await requestRepository.SaveChangesAsync();
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
                    rs.""Title"", rs.""Id""
                FROM ""Requests"" r
                LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                        var countSql = new StringBuilder(@"SELECT COUNT(*) FROM ""Requests"" r
                    LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                var parameters = new DynamicParameters();
                var conditions = new List<string>();

                conditions.Add("r.\"IsDeleted\" = 0");
                conditions.Add("r.\"Status\" != 0");

                if (!string.IsNullOrWhiteSpace(dto.Text))
                {
                    var searchableColumns = new List<string>
                    {
                        "InquiryType", "InquiryField", "CompanyName", "Department", "ResponsiblePerson",
                        "ClientCompany", "Email", "ProcessingStatus", "FinalResult", "Notes", "Date",
                        "Client", "ContactNumber", "ProjectDetails"
                    };

                    var searchConditions = new List<string>();
                    foreach (var column in searchableColumns)
                    {
                        searchConditions.Add($"r.\"{column}\" ILIKE @searchText");
                    }

                    conditions.Add($"({string.Join(" OR ", searchConditions)})");
                    parameters.Add("@searchText", $"%{dto.Text}%");
                }

                if (dto?.Category != null)
                {
                    conditions.Add("r.\"RequestStatusId\" = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.Category);
                }

                if (dto?.Status != null)
                {
                    conditions.Add("r.\"Status\" = @Status");
                    parameters.Add("@Status", dto.Status);
                }

                if (dto?.Priority != null)
                {
                    conditions.Add("r.\"Priority\" = @Priority");
                    parameters.Add("@Priority", dto.Priority);
                }

                if (conditions.Any())
                {
                    sql.Append(" WHERE " + string.Join(" AND ", conditions));
                    countSql.Append(" WHERE " + string.Join(" AND ", conditions));
                }

                sql.Append(" ORDER BY r.\"CreatedAt\" DESC");

                int totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);

                if (totalCount == 0)
                {
                    return PagedResult<RequestModel>.Create(new List<RequestModel>(), 0, dto.PageSize, 0, dto.PageIndex, 0);
                }

                if (dto.PageIndex == 0) dto.PageIndex = 1;
                if (dto.PageSize == 0) dto.PageSize = totalCount;

                int itemsPerPage = dto.PageSize;
                int totalPages = (totalCount / itemsPerPage) + (totalCount % itemsPerPage == 0 ? 0 : 1);

                if (dto.PageIndex > totalPages)
                {
                    dto.PageIndex = totalPages;
                }

                int skip = (dto.PageIndex - 1) * dto.PageSize;
                sql.Append(" LIMIT @PageSize OFFSET @Offset");
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

                return PagedResult<RequestModel>.Create(list.ToList(), totalCount, dto.PageSize, list.Count(), dto.PageIndex, totalPages);
            }
        }


        public async ValueTask<PagedResult<RequestModel>> GetDeletedRequeststAsync(RequestForFilterDTO dto)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            using (var db = new NpgsqlConnection(connectionString))
            {
                var sql = new StringBuilder(@"
                SELECT 
                    r.*, 
                    rs.""Id"" AS RequestStatus_Id, 
                    rs.""Title"", rs.""Id""
                FROM ""Requests"" r
                LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                var countSql = new StringBuilder(@"SELECT COUNT(*) FROM ""Requests"" r
                    LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                var parameters = new DynamicParameters();
                var conditions = new List<string>();

                conditions.Add("r.\"IsDeleted\" = 1");

                if (!string.IsNullOrWhiteSpace(dto.Text))
                {
                    var searchableColumns = new List<string>
                    {
                        "InquiryType", "InquiryField", "CompanyName", "Department", "ResponsiblePerson",
                        "ClientCompany", "Email", "ProcessingStatus", "FinalResult", "Notes", "Date",
                        "Client", "ContactNumber", "ProjectDetails"
                    };

                    var searchConditions = new List<string>();
                    foreach (var column in searchableColumns)
                    {
                        searchConditions.Add($"r.\"{column}\" ILIKE @searchText");
                    }

                    conditions.Add($"({string.Join(" OR ", searchConditions)})");
                    parameters.Add("@searchText", $"%{dto.Text}%");
                }

                if (dto?.Category != null)
                {
                    conditions.Add("r.\"RequestStatusId\" = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.Category);
                }

                // Append WHERE clause if there are conditions
                if (conditions.Any())
                {
                    sql.Append(" WHERE " + string.Join(" AND ", conditions));
                    countSql.Append(" WHERE " + string.Join(" AND ", conditions));
                }

                sql.Append(" ORDER BY r.\"CreatedAt\" DESC");


                // Pagination
                int totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);

                if (totalCount == 0)
                {
                    return PagedResult<RequestModel>.Create(new List<RequestModel>(), 0, dto.PageSize, 0, dto.PageIndex, 0);
                }

                if (dto.PageIndex == 0) dto.PageIndex = 1;
                if (dto.PageSize == 0) dto.PageSize = totalCount;

                int itemsPerPage = dto.PageSize;
                int totalPages = (totalCount / itemsPerPage) + (totalCount % itemsPerPage == 0 ? 0 : 1);

                if (dto.PageIndex > totalPages)
                {
                    dto.PageIndex = totalPages;
                }

                int skip = (dto.PageIndex - 1) * dto.PageSize;
                sql.Append(" LIMIT @PageSize OFFSET @Offset");
                parameters.Add("@PageSize", dto.PageSize);
                parameters.Add("@Offset", skip);

                // Fetch data
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

                return PagedResult<RequestModel>.Create(list.ToList(), totalCount, dto.PageSize, list.Count(), dto.PageIndex, totalPages);
            }
        }


        public async ValueTask<PagedResult<RequestModel>> GetPendingRequeststAsync(RequestForFilterDTO dto)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            using (var db = new NpgsqlConnection(connectionString))
            {
                var sql = new StringBuilder(@"
                SELECT 
                    r.*, 
                    rs.""Id"" AS RequestStatus_Id, 
                    rs.""Title"", rs.""Id""
                FROM ""Requests"" r
                LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                var countSql = new StringBuilder(@"SELECT COUNT(*) FROM ""Requests"" r
                    LEFT JOIN ""RequestStatuses"" rs ON r.""RequestStatusId"" = rs.""Id""
                ");

                var parameters = new DynamicParameters();
                var conditions = new List<string>();

                conditions.Add("r.\"IsDeleted\" = 0");
                conditions.Add("r.\"Status\" = 0");

                if (!string.IsNullOrWhiteSpace(dto.Text))
                {
                    var searchableColumns = new List<string>
                    {
                        "InquiryType", "InquiryField", "CompanyName", "Department", "ResponsiblePerson",
                        "ClientCompany", "Email", "ProcessingStatus", "FinalResult", "Notes", "Date",
                        "Client", "ContactNumber", "ProjectDetails"
                    };

                    var searchConditions = new List<string>();
                    foreach (var column in searchableColumns)
                    {
                        searchConditions.Add($"r.\"{column}\" ILIKE @searchText");
                    }

                    conditions.Add($"({string.Join(" OR ", searchConditions)})");
                    parameters.Add("@searchText", $"%{dto.Text}%");
                }

                if (dto?.Category != null)
                {
                    conditions.Add("r.\"RequestStatusId\" = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.Category);
                }

                conditions.Add($"r.\"Status\" = 0");

                if (conditions.Any())
                {
                    sql.Append(" WHERE " + string.Join(" AND ", conditions));
                    countSql.Append(" WHERE " + string.Join(" AND ", conditions));
                }
                sql.Append(" ORDER BY r.\"CreatedAt\" DESC");

                // Pagination
                int totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);

                if (totalCount == 0)
                {
                    return PagedResult<RequestModel>.Create(new List<RequestModel>(), 0, dto.PageSize, 0, dto.PageIndex, 0);
                }


                if (dto.PageIndex == 0) dto.PageIndex = 1;
                if (dto.PageSize == 0) dto.PageSize = totalCount;

                int itemsPerPage = dto.PageSize;
                int totalPages = (totalCount / itemsPerPage) + (totalCount % itemsPerPage == 0 ? 0 : 1);

                if (dto.PageIndex > totalPages)
                {
                    dto.PageIndex = totalPages;
                }

                int skip = (dto.PageIndex - 1) * dto.PageSize;
                sql.Append(" LIMIT @PageSize OFFSET @Offset");
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
                        ProjectDetails = item.ProjectDetails,
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

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.CreateRequest);

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

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.UpdateRequest);

            return true;
        }

        public async ValueTask<bool> DeleteRequest(int id)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 0).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            existRequest.IsDeleted = 1;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.DeleteRequest);

            return true;
        }

        public async ValueTask<bool> RecoverRequest(int id)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 1).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            existRequest.IsDeleted = 0;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.RestoreRequest);

            return true;
        }

        public async ValueTask<List<RequestFilterModel>> GetFilterValue(RequestStatusForFilterDTO dto)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            using (var db = new NpgsqlConnection(connectionString))
            {
                var query = "SELECT * FROM \"Requests\" WHERE \"IsDeleted\" = @IsDeleted";
                if (dto.Status is not null)
                {
                    query += " AND \"Status\" = @Status";
                }

                var parameters = new { IsDeleted = dto.IsDeleted, Status = dto.Status };
                var existRequest = (await db.QueryAsync<Domain.Entities.Requests.Request>(query, parameters)).ToList();
                var groupedFilters = new List<RequestFilterModel>();
                var fields = new Dictionary<string, Func<Domain.Entities.Requests.Request, string>>
                {
                    { "Client", x => x.Client },
                    { "ClientCompany", x => x.ClientCompany },
                    { "CompanyName", x => x.CompanyName },
                    { "ContactNumber", x => x.ContactNumber },
                    { "Notes", x => x.Notes },
                    { "ProjectDetails", x => x.ProjectDetails },
                    { "Date", x => x.Date },
                    { "Department", x => x.Department },
                    { "Email", x => x.Email },
                    { "InquiryField", x => x.InquiryField },
                    { "InquiryType", x => x.InquiryType },
                    { "FinalResult", x => x.FinalResult },
                    { "ProcessingStatus", x => x.ProcessingStatus },
                    { "ResponsiblePerson", x => x.ResponsiblePerson },
                    { "Priority", x => x.Priority.ToString() },
                    { "Deadline", x => x?.Deadline?.ToString() },
                    { "Status", x => x.Status.ToString() },
                };

                foreach (var field in fields)
                {
                    var uniqueValues = existRequest
                    .Select(x => field.Value(x))
                    .Where(value => !string.IsNullOrWhiteSpace(value))
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

        public async ValueTask<bool> ChangeRequestStatus(int requestId, bool status)
        {
            var existRequest = await requestRepository.GetAsync(x => x.Id == requestId);

            existRequest.Status = status ? Domain.Enum.ProjectStatus.InProgress : ProjectStatus.Rejected;
            existRequest.IsDeleted = status ? 0 : 1;

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();

            var verificationMessage = new EmailMessage();

            if(status)
                verificationMessage = EmailMessage.SuccessSendRequest(existRequest.Email, existRequest.ResponsiblePerson);
            else
                verificationMessage = EmailMessage.DenySendRequest(existRequest.Email, existRequest.ResponsiblePerson);

            emailInboxService.EnqueueEmail(verificationMessage);

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.ChangeRequestStatus);

            return true;
        }



        public async ValueTask<List<RequestsCountModel>> GetRequestsCount()
        {
            var requests = await requestRepository.GetAll(x => x.IsDeleted == 0).Include(x => x.RequestStatus).ToListAsync();

            var statusCounts = Enum.GetValues(typeof(ProjectStatus))
            .Cast<ProjectStatus>()
            .Select(status => new RequestsCountModel
            {
                Title = status.ToString(),
                Count = requests.Count(r => r.Status == status)
            })
            .ToList();

            var priorityCounts = Enum.GetValues(typeof(Priority))
                .Cast<Priority>()
                .Select(priority => new RequestsCountModel
                {
                    Title = priority.ToString(),
                    Count = requests.Count(r => r.Priority == priority)
                })
                .ToList();

            var requestStatusCounts = requests
                .GroupBy(x => x.RequestStatus.Title)
                .Select(g => new RequestsCountModel
                {
                    Title = g.Key,
                    Count = g.Count()
                })
                .ToList();


            var requestAllCounts = requests
                .GroupBy(x => x.IsDeleted == 0)
                .Select(g => new RequestsCountModel
                {
                    Title = "all",
                    Count = requests.Count()
                })
                .ToList();

            var allRequests = new List<RequestsCountModel>();
            allRequests.AddRange(statusCounts);
            allRequests.AddRange(priorityCounts);
            allRequests.AddRange(requestStatusCounts);
            allRequests.AddRange(requestAllCounts);

            return allRequests;
        }

    }
}
