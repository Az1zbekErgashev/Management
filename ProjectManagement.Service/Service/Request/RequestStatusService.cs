using Dapper;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using System.Text;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace ProjectManagement.Service.Service.Request
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
                Title = dto.Titile,
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


            existStatus.Title = dto.Titile;

            requestStatusRepository.UpdateAsync(existStatus);
            await requestStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<PagedResult<RequestModel>> GetRequeststAsync(RequestForFilterDTO dto)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            using (var db = new NpgsqlConnection(connectionString))
            {
                var sql = new StringBuilder("SELECT * FROM \"Requests\" WHERE 1=1");
                var countSql = new StringBuilder("SELECT COUNT(*) FROM \"Requests\" WHERE 1=1");

                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(dto.Category))
                {
                    sql.Append(" AND category ILIKE @Category");
                    countSql.Append(" AND category ILIKE @Category");
                    parameters.Add("@Category", $"%{dto.Category}%");
                }

                if (!string.IsNullOrEmpty(dto.InquiryType))
                {
                    sql.Append(" AND InquiryType ILIKE @InquiryType");
                    countSql.Append(" AND InquiryType ILIKE @InquiryType");
                    parameters.Add("@InquiryType", $"%{dto.InquiryType}%");
                }

                if (!string.IsNullOrEmpty(dto.InquiryField))
                {
                    sql.Append(" AND InquiryField ILIKE @InquiryField");
                    countSql.Append(" AND InquiryField ILIKE @InquiryField");
                    parameters.Add("@InquiryField", $"%{dto.InquiryField}%");
                }

                if (!string.IsNullOrEmpty(dto.CompanyName))
                {
                    sql.Append(" AND CompanyName ILIKE @CompanyName");
                    countSql.Append(" AND CompanyName ILIKE @CompanyName");
                    parameters.Add("@CompanyName", $"%{dto.CompanyName}%");
                }

                if (!string.IsNullOrEmpty(dto.Department))
                {
                    sql.Append(" AND Department ILIKE @Department");
                    countSql.Append(" AND Department ILIKE @Department");
                    parameters.Add("@Department", $"%{dto.Department}%");
                }

                if (!string.IsNullOrEmpty(dto.ResponsiblePerson))
                {
                    sql.Append(" AND ResponsiblePerson ILIKE @ResponsiblePerson");
                    countSql.Append(" AND ResponsiblePerson ILIKE @ResponsiblePerson");
                    parameters.Add("@ResponsiblePerson", $"%{dto.ResponsiblePerson}%");
                }

                if (!string.IsNullOrEmpty(dto.ClientCompany))
                {
                    sql.Append(" AND ClientCompany ILIKE @ClientCompany");
                    countSql.Append(" AND ClientCompany ILIKE @ClientCompany");
                    parameters.Add("@ClientCompany", $"%{dto.ClientCompany}%");
                }

                if (!string.IsNullOrEmpty(dto.Email))
                {
                    sql.Append(" AND Email ILIKE @Email");
                    countSql.Append(" AND Email ILIKE @Email");
                    parameters.Add("@Email", $"%{dto.Email}%");
                }

                if (!string.IsNullOrEmpty(dto.ProcessingStatus))
                {
                    sql.Append(" AND ProcessingStatus ILIKE @ProcessingStatus");
                    countSql.Append(" AND ProcessingStatus ILIKE @ProcessingStatus");
                    parameters.Add("@ProcessingStatus", $"%{dto.ProcessingStatus}%");
                }

                if (!string.IsNullOrEmpty(dto.FinalResult))
                {
                    sql.Append(" AND FinalResult ILIKE @FinalResult");
                    countSql.Append(" AND FinalResult ILIKE @FinalResult");
                    parameters.Add("@FinalResult", $"%{dto.FinalResult}%");
                }

                if (!string.IsNullOrEmpty(dto.Notes))
                {
                    sql.Append(" AND Notes ILIKE @Notes");
                    countSql.Append(" AND Notes ILIKE @Notes");
                    parameters.Add("@Notes", $"%{dto.Notes}%");
                }

                if (dto?.RequestStatusId != null)
                {
                    sql.Append(" AND RequestStatusId = @RequestStatusId");
                    countSql.Append(" AND RequestStatusId = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.RequestStatusId);
                }

                if (dto?.CreatedAt != null)
                {
                    sql.Append(" AND CreatedAt >= @CreatedAt");
                    countSql.Append(" AND CreatedAt >= @CreatedAt");
                    parameters.Add("@CreatedAt", dto.CreatedAt);
                }

                if (!string.IsNullOrEmpty(dto.SortBy) && !string.IsNullOrEmpty(dto.Order))
                {
                    sql.Append($" ORDER BY {dto.SortBy} {(dto.Order.ToLower() == "ascend" ? "ASC" : "DESC")}");
                }

                int totalCount = await db.ExecuteScalarAsync<int>(countSql.ToString(), parameters);

                if (totalCount == 0)
                {
                    return PagedResult<RequestModel>.Create(
                        new List<RequestModel>(), 0, dto.PageSize, 0, dto.PageIndex, 0);
                }

                int skip = (dto.PageIndex - 1) * dto.PageSize;
                int take = dto.PageSize;
                sql.Append(" LIMIT @PageSize OFFSET @Offset");
                parameters.Add("@PageSize", take);
                parameters.Add("@Offset", skip);

                var list = await db.QueryAsync<RequestModel>(sql.ToString(), parameters);
                int totalPages = (int)Math.Ceiling((double)totalCount / take);

                return PagedResult<RequestModel>.Create(list.ToList(), totalCount, take, list.Count(), dto.PageIndex, totalPages);
            }
        }


        public async ValueTask<string> CreateRequestAsync(List<RequestForCreateDTO> dto)
        {
            var jsonFilePath = Path.Combine("wwwroot", "images", "request.json");

            if (!File.Exists(jsonFilePath))
            {
                Console.WriteLine("Файл JSON не найден.");
                return "SAS";
            }


            string jsonContent = await File.ReadAllTextAsync(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
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
                    CreatedAt = item.CreatedAt is not null ?  DateTime.ParseExact(item.CreatedAt, "dd.MM.yyyy", CultureInfo.InvariantCulture).ToUniversalTime() : null,
                    RequestStatusId = 1
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
                RequestStatusId = dto.RequestStatusId
            };

            await requestRepository.CreateAsync(request);

            await requestRepository.SaveChangesAsync();

            return true;
        }


        public async ValueTask<bool> UpdateRequest(int id, RequestForCreateDTO dto)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 0).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if(existRequest is null) throw new ProjectManagementException(404, "request_not_found");

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
            existRequest.UpdatedAt = DateTime.UtcNow;
            existRequest.RequestStatusId = dto.RequestStatusId;
            existRequest.CreatedAt = dto.CreateAtForRequest is not null ? dto.CreateAtForRequest : existRequest.CreatedAt;

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

    }
}
