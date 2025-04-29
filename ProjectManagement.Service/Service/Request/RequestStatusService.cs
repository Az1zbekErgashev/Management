using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.Attachment;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using ProjectManagement.Service.StringExtensions;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
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
        private readonly IAttachmentService attachmentService;
        private readonly IGenericRepository<Comments> commentstService;
        private readonly IGenericRepository<RequestHistory> requestHistory;
        public RequestStatusService(
            IGenericRepository<RequestStatus> requestStatusRepository,
            IGenericRepository<Domain.Entities.Requests.Request> requestRepository,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IGenericRepository<Logs> logRepository,
            IEmailInboxService emailInboxService
,
            IAttachmentService attachmentService,
            IGenericRepository<Comments> commentstService,
            IGenericRepository<RequestHistory> requestHistory)
        {
            this.requestStatusRepository = requestStatusRepository;
            this.requestRepository = requestRepository;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _logRepository = logRepository;
            this.emailInboxService = emailInboxService;
            this.attachmentService = attachmentService;
            this.commentstService = commentstService;
            this.requestHistory = requestHistory;
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

            existStatus.IsDeleted = 1;

            foreach(var item in requestsCount)
            {
                item.IsDeleted = 1;
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

                if (!string.IsNullOrWhiteSpace(dto.Text))
                {
                    var searchableColumns = new List<string>
                {
                    "InquiryType", "InquiryField", "CompanyName", "Department", "ResponsiblePerson",
                    "ClientCompany", "Email", "ProcessingStatus", "Status", "Notes", "Date", "LastUpdated",
                    "Client", "ContactNumber", "ProjectDetails"
                };

                    var searchConditions = new List<string>();
                    foreach (var column in searchableColumns)
                    {
                        searchConditions.Add($"r.\"{column}\" ILIKE @searchText");
                    }

                    // Если есть фильтр по категории, добавляем его в условие поиска текста
                    if (dto?.Category != null)
                    {
                        searchConditions.Add("r.\"RequestStatusId\" = @RequestStatusId");
                        parameters.Add("@RequestStatusId", dto.Category);
                    }

                    conditions.Add($"({string.Join(" OR ", searchConditions)})");
                    parameters.Add("@searchText", $"%{dto.Text}%");
                }
                else if (dto?.Category != null)
                {
                    // Если текста нет, но есть категория, добавляем её как отдельное условие
                    conditions.Add("r.\"RequestStatusId\" = @RequestStatusId");
                    parameters.Add("@RequestStatusId", dto.Category);
                }

                if (dto?.Status != null)
                {
                    conditions.Add("r.\"Status\" = @Status");
                    parameters.Add("@Status", dto.Status);
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


        public async ValueTask<RequestModel> GetRequestById(int id)
        {
            var request = await requestRepository
                .GetAll(x => x.Id == id)
                .Include(x => x.History)
                .ThenInclude(x => x.User)
                .Include(x => x.Comments.Where(x => x.IsDeleted == 0))
                .ThenInclude(x => x.User)
                .Include(x => x.RequestStatus)
                .Include(x => x.File)
                .FirstOrDefaultAsync();


            if(request is null) throw new ProjectManagementException(404, "request_not_found");

            return new RequestModel().MapFromEntity(request);
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
                        "ClientCompany", "Email", "ProcessingStatus", "Status", "Notes", "Date", "LastUpdated",
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

        public async ValueTask<bool> CreateRequest(RequestForCreateDTO dto)
        {
            Domain.Entities.Attachment.Attachment attachment = null;

            if (dto.File is not null)
            {
                attachment = await attachmentService.UploadAsync(dto.File.ToAttachmentOrDefault());
            }

            var request = new Domain.Entities.Requests.Request
            {
                Client = dto.Client,
                ClientCompany = dto.ClientCompany,
                CompanyName = dto.CompanyName,
                ContactNumber = dto.ContactNumber,
                Department = dto.Department,
                Email = dto.Email,
                InquiryField = dto.InquiryField,
                InquiryType = dto.InquiryType,
                ProcessingStatus = dto.ProcessingStatus,
                Notes = dto.Notes,
                ProjectDetails = dto.ProjectDetails,
                ResponsiblePerson = dto.ResponsiblePerson,
                CreatedAt = DateTime.UtcNow,
                RequestStatusId = dto.RequestStatusId,
                Date = dto.Date,
                Status = dto.Status,
                File = attachment,
                FileId = attachment?.Id,
                LastUpdated = dto?.LastUpdated?.ToString(),
            };

            await requestRepository.CreateAsync(request);

            await requestRepository.SaveChangesAsync();

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.CreateRequest);
            await StringExtensions.StringExtensions.SaveRequestHistory(requestHistory, RequestLog.CreateRequest, _httpContextAccessor, request.Id, RequestLogType.Create);

            return true;
        }

        public async ValueTask<bool> UpdateRequest(int id, RequestForCreateDTO dto)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id && x.IsDeleted == 0).Include(x => x.File).Include(x => x.RequestStatus).FirstOrDefaultAsync();

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            Domain.Entities.Attachment.Attachment attachment = existRequest.File;

            if (dto.UpdateFile)
            {
                attachment = await attachmentService.UploadAsync(dto.File.ToAttachmentOrDefault());
            }
            else if (dto.RemoveFile)
            {
                attachment = null;
            }

            existRequest.Client = dto.Client;
            existRequest.ClientCompany = dto.ClientCompany;
            existRequest.CompanyName = dto.CompanyName;
            existRequest.ContactNumber = dto.ContactNumber;
            existRequest.Department = dto.Department;
            existRequest.Email = dto.Email;
            existRequest.InquiryField = dto.InquiryField;
            existRequest.InquiryType = dto.InquiryType;
            existRequest.ProcessingStatus = dto.ProcessingStatus;
            existRequest.Notes = dto.Notes;
            existRequest.ProjectDetails = dto.ProjectDetails;
            existRequest.ResponsiblePerson = dto.ResponsiblePerson;
            existRequest.Date = dto.Date;
            existRequest.Status = dto.Status;
            existRequest.FileId = attachment?.Id;
            existRequest.RequestStatusId = dto.RequestStatusId;
            existRequest.UpdatedAt = DateTime.UtcNow;
            existRequest.CreatedAt = existRequest.CreatedAt ?? DateTime.UtcNow;
            existRequest.LastUpdated = dto?.LastUpdated?.ToString();

            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();

            await StringExtensions.StringExtensions.SaveLogAsync(_logRepository, _httpContextAccessor, Domain.Enum.LogAction.UpdateRequest);
            await StringExtensions.StringExtensions.SaveRequestHistory(requestHistory, RequestLog.UpdateRequest, _httpContextAccessor, id, RequestLogType.Update);

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
            await StringExtensions.StringExtensions.SaveRequestHistory(requestHistory, RequestLog.DeleteRequest, _httpContextAccessor, id, RequestLogType.Delete);
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
                    { "ProcessingStatus", x => x.ProcessingStatus },
                    { "ResponsiblePerson", x => x.ResponsiblePerson },
                    { "Status", x => x.Status },
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
        public async ValueTask<List<RequestsCountModel>> GetRequestsCount()
        {
            var requests = await requestRepository.GetAll(x => x.IsDeleted == 0).Include(x => x.RequestStatus).ToListAsync();

            var statusCounts = requests
            .Where(x => x.Status != null)
            .GroupBy(x => x.Status)
            .Select(status => new RequestsCountModel
            {
                Title = status?.ToString(),
                Count = status.Count()
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
            allRequests.AddRange(requestStatusCounts);
            allRequests.AddRange(requestAllCounts);

            return allRequests;
        }
        public async ValueTask<List<CommentsModel>> GetCommentsAsync(CommentsForFilterDTO dto)
        {
            var allComments = await commentstService
                .GetAll(x => x.RequestId == dto.RequestId && x.IsDeleted == 0)
                .Include(x => x.User)
                    .ThenInclude(u => u.Image)
                    .Include(x => x.Replies).ThenInclude(x => x.User).ThenInclude(x => x.Image)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var rootComments = allComments
                .Where(x => x.ParentCommentId == null)
                .Take(dto.PageSize)
                .ToList();

            var rootModels = rootComments
                .Select(rootComment => new CommentsModel().MapFromEntity(rootComment))
                .ToList();

            foreach (var rootModel in rootModels)
            {
                var applyReplyLimit = dto.RootCommentId.HasValue && rootModel.Id == dto.RootCommentId;

                var allReplies = new List<CommentsModel>();
                var repliesToProcess = allComments
                    .Where(x => x.ParentCommentId == rootModel.Id)
                    .ToList();

                var queue = new Queue<Comments>(repliesToProcess); // Replace CommentEntity with actual type
                while (queue.Count > 0 && (!applyReplyLimit || allReplies.Count < dto.ReplyPageSize))
                {
                    var reply = queue.Dequeue();
                    var replyModel = new CommentsModel().MapFromEntity(reply);
                    allReplies.Add(replyModel);

                    var nestedReplies = allComments
                        .Where(x => x.ParentCommentId == reply.Id)
                        .ToList();

                    foreach (var nestedReply in nestedReplies)
                    {
                        if (!applyReplyLimit || allReplies.Count < dto.ReplyPageSize)
                        {
                            queue.Enqueue(nestedReply);
                        }
                    }
                }

                var repliesLimit = applyReplyLimit ? Math.Max(0, dto.ReplyPageSize) : allReplies.Count;
                rootModel.Replies = allReplies
                    .OrderBy(x => x.CreatedAt)
                    .Take(repliesLimit)
                    .ToList();
            }

            return rootModels;
        }
        public async ValueTask<PagedResult<RequestHistoryModel>> GetRequestHistoryAsync(CommentsForFilterDTO dto)
        {
            var comments = requestHistory.GetAll(x => x.RequestId == dto.RequestId && x.IsDeleted == 0).Include(x => x.User).ThenInclude(x => x.Image).OrderBy(x => x.Id).AsQueryable();

            int totalCount = comments.Count();

            if (totalCount == 0)
            {
                return PagedResult<RequestHistoryModel>.Create(
                    Enumerable.Empty<RequestHistoryModel>(),
                    0,
                    dto.PageSize,
                    0,
                    dto.PageIndex,
                    0
                );
            }

            if (dto.PageIndex == 0)
            {
                dto.PageIndex = 1;
            }

            if (dto.PageSize == 0)
            {
                dto.PageSize = totalCount;
            }

            int itemsPerPage = dto.PageSize;
            int totalPages = (totalCount / itemsPerPage) + (totalCount % itemsPerPage == 0 ? 0 : 1);

            comments = comments.ToPagedList(dto);

            var list = await comments.ToListAsync();

            List<RequestHistoryModel> models = list.Select(
                f => new RequestHistoryModel().MapFromEntity(f))
                .ToList();

            return PagedResult<RequestHistoryModel>.Create(models,
                totalCount,
                itemsPerPage,
                models.Count,
                dto.PageIndex,
                totalPages
                );
        }
        public async ValueTask<bool> CreateComment(CommentForCreateDTO dto)
        {
            var existRequest = await requestRepository.GetAsync(x => x.Id == dto.RequestId);

            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");

            var context = _httpContextAccessor.HttpContext;

            if (!int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                throw new InvalidCredentialException();
            }

            Comments? parentComment = null;
            if (dto.ParentCommentId.HasValue)
            {
                parentComment = await commentstService.GetAsync(x => x.Id == dto.ParentCommentId.Value);
                if (parentComment is null)
                    throw new ProjectManagementException(404, "parent_comment_not_found");
            }

            var newComment = new Comments
            {
                RequestId = existRequest.Id,
                Text = dto.Text,
                UserId = userId,
                ParentCommentId = dto.ParentCommentId, 
                CreatedAt = DateTime.UtcNow
            };

            await commentstService.CreateAsync(newComment);

            if (parentComment != null)
            {
                parentComment.Replies ??= new List<Comments>();
                parentComment.Replies.Add(newComment);
                commentstService.UpdateAsync(parentComment);
            }

            await commentstService.SaveChangesAsync();
            return true;
        }
        public async ValueTask<bool> UpdateComment(CommentForCreateDTO dto)
        {
            var existComment = await commentstService.GetAsync(x => x.Id == dto.CommentId);
            if (existComment is null) throw new ProjectManagementException(404, "comment_not_found");

            existComment.UpdatedAt = DateTime.UtcNow;
            existComment.Text = dto.Text;

            if (dto.ParentCommentId.HasValue)
            {
                var parentComment = await commentstService.GetAsync(x => x.Id == dto.ParentCommentId.Value);
                if (parentComment is null)
                    throw new ProjectManagementException(404, "parent_comment_not_found");

                existComment.ParentCommentId = dto.ParentCommentId;
            }
            else
            {
                existComment.ParentCommentId = null;
            }

            commentstService.UpdateAsync(existComment);
            await commentstService.SaveChangesAsync();

            return true;
        }
        public async ValueTask<bool> DeleteComment(int commentId)
        {
            var existComment = await commentstService.GetAsync(x => x.Id == commentId);
            if (existComment is null) throw new ProjectManagementException(404, "comment_not_found");

            existComment.UpdatedAt = DateTime.UtcNow;
            existComment.IsDeleted = 1;

            commentstService.UpdateAsync(existComment);
            await commentstService.SaveChangesAsync();
            return true;
        }


        public async ValueTask<List<RequestRateModel>> GetRequestProcent()
        {
            var allRequestsRaw = await requestRepository.GetAll(x => x.IsDeleted == 0 && x.Status != null).Include(x => x.RequestStatus).ToListAsync();

            var totalCount = allRequestsRaw.Count;
            var madeCount = allRequestsRaw.Count(x => x.Status == "Made");
            var result = new List<RequestRateModel>();
            var totalPercent = totalCount > 0 ? (int)Math.Round((double)madeCount / totalCount * 100) : 0;
            result.Add(new RequestRateModel().MapFromEntity("all_requests", totalPercent, totalCount));
            var grouped = allRequestsRaw
            .GroupBy(x => x.RequestStatus)
            .ToList();

            foreach (var group in grouped)
            {
                var categoryText = group.Key?.Title;
                var categoryTotal = group.Count();
                var categoryMade = group.Count(x => x.Status == "Made");
                var categoryPercent = categoryTotal > 0 ? (int)Math.Round((double)categoryMade / categoryTotal * 100) : 0;

                result.Add(new RequestRateModel().MapFromEntity(categoryText, categoryPercent, categoryTotal));
            }

            return result;
        }

        public async ValueTask<List<RequestCountByStatusModel>> GetStatusCounts()
        {
            var allRequestsRaw = await requestRepository.GetAll(x => x.IsDeleted == 0 && x.Status != null).Include(x => x.RequestStatus).ToListAsync();

            var groupedRequests = allRequestsRaw
            .GroupBy(x => x.RequestStatusId)
            .ToList();

            var result = new List<RequestCountByStatusModel>();

            var allRequestsModel = new RequestCountByStatusModel
            {
                CategoryText = "all_requests",
                Counts = new List<StatusCountItem>
                {
                    new StatusCountItem { Status = "Failed", Count = allRequestsRaw.Count(x => x.Status == "Failed") },
                    new StatusCountItem { Status = "Made", Count = allRequestsRaw.Count(x => x.Status == "Made") },
                    new StatusCountItem { Status = "On-going", Count = allRequestsRaw.Count(x => x.Status == "On-going") },
                    new StatusCountItem { Status = "On-hold", Count = allRequestsRaw.Count(x => x.Status == "On-hold") },
                    new StatusCountItem { Status = "Dropped", Count = allRequestsRaw.Count(x => x.Status == "Dropped") },
                },
                Total = allRequestsRaw.Count()
            };

            result.Add(allRequestsModel);

            foreach (var group in groupedRequests)
            {
                var requestList = group.ToList(); 

                var model = new RequestCountByStatusModel().MapFromEntity(requestList);

                result.Add(model);
            }

            return result;
        }

        public async ValueTask<List<int>> GetAvailableYears()
        {
            var allRequests = await requestRepository
                .GetAll(x => x.IsDeleted == 0 && x.Status != null)
                .ToListAsync();

            var years = allRequests
                .Where(x => DateTime.TryParse(x.Date, out _))
                .Select(x => DateTime.Parse(x.Date).Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            return years;
        }

        public async ValueTask<List<Dictionary<string, object>>> GetMonthlyChartData(int year)
        {
            var allRequests = await requestRepository
                .GetAll(x => x.IsDeleted == 0 && x.Status != null)
                .ToListAsync();

            var filtered = allRequests
                .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Year == year)
                .GroupBy(x => DateTime.Parse(x.Date).Month)
                .OrderBy(x => x.Key);

            var result = new List<Dictionary<string, object>>();

            foreach (var group in filtered)
            {
                var month = group.Key; 

                var dict = new Dictionary<string, object>
                {
                    ["month"] = month,
                    ["Made"] = group.Count(x => x.Status == "Made"),
                    ["Failed"] = group.Count(x => x.Status == "Failed"),
                    ["On-going"] = group.Count(x => x.Status == "On-going"),
                    ["OnHold"] = group.Count(x => x.Status == "On-hold"),
                    ["Dropped"] = group.Count(x => x.Status == "Dropped")
                };

                result.Add(dict);
            }

            return result;
        }
    }
}
