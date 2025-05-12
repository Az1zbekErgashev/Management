using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NPOI.HSSF.Record.Aggregates;
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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
        private readonly IGenericRepository<ProcessingStatus> processingStatus;
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
            IGenericRepository<RequestHistory> requestHistory,
            IGenericRepository<ProcessingStatus> processingStatus)
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
            this.processingStatus = processingStatus;
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
            var query = requestRepository.GetAll()
               .Include(x => x.ProcessingStatus)
               .Include(x => x.RequestStatus)
               .AsQueryable();

            query = query.OrderBy(x => x.Id);

            if (dto.Category != null)
            {
                query = query.Where(x => x.RequestStatusId == dto.Category);
            }

            if (!string.IsNullOrEmpty(dto.Text))
            {
                string searchText = $"%{dto.Text}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.InquiryField, searchText) ||
                    EF.Functions.Like(x.Email, searchText) ||
                    EF.Functions.Like(x.ProjectDetails, searchText) ||
                    EF.Functions.Like(x.ResponsiblePerson, searchText) ||
                    EF.Functions.Like(x.Date, searchText) ||
                    EF.Functions.Like(x.ContactNumber, searchText) ||
                    EF.Functions.Like(x.CompanyName, searchText) ||
                    EF.Functions.Like(x.Notes, searchText) ||
                    EF.Functions.Like(x.Client, searchText) ||
                    EF.Functions.Like(x.ClientCompany, searchText) ||
                    EF.Functions.Like(x.Department, searchText) ||
                    EF.Functions.Like(x.InquiryType, searchText) ||
                    EF.Functions.Like(x.Status, searchText) ||
                    EF.Functions.Like(x.ProcessingStatus == null ? x.ProcessingStatus.Text : "", searchText) ||
                    EF.Functions.Like(x.LastUpdated, searchText));
            }

            if (dto.IsDeleted != null)
            {
                query = query.Where(x => x.IsDeleted == dto.IsDeleted);
            }

            int totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                return PagedResult<RequestModel>.Create(
                    Enumerable.Empty<RequestModel>(),
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

            query = query.ToPagedList(dto);

            var list = await query.ToListAsync();

            List<RequestModel> models = list.Select(
                f => new RequestModel().MapFromEntity(f))
                .ToList();


            var pagedResult = PagedResult<RequestModel>.Create(models,
                totalCount,
                itemsPerPage,
                models.Count,
                dto.PageIndex,
                totalPages
                );

            return pagedResult;
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
                .Include(x => x.ProcessingStatus)
                .FirstOrDefaultAsync();

     
            if (request is null) throw new ProjectManagementException(404, "request_not_found");

            return new RequestModel().MapFromEntity(request);
        }

        public async ValueTask<bool> CreateRequest(RequestForCreateDTO dto)
        {
            Domain.Entities.Attachment.Attachment attachment = null;

            if (dto.File is not null)
            {
                attachment = await attachmentService.UploadAsync(dto.File.ToAttachmentOrDefault());
            }

            var existCategory = await requestStatusRepository.GetAll(x => x.Id == dto.RequestStatusId && x.IsDeleted == 0).FirstOrDefaultAsync();
            var request = new Domain.Entities.Requests.Request
            {
                Client = dto.Client,
                ClientCompany = dto.ClientCompany,
                CompanyName = existCategory.Title,
                ContactNumber = dto.ContactNumber,
                Department = dto.Department,
                Email = dto.Email,
                InquiryField = dto.InquiryField,
                InquiryType = dto.InquiryType,
                ProcessingStatusId = dto.ProcessingStatus,
                Notes = dto.Notes,
                ProjectDetails = dto.ProjectDetails,
                ResponsiblePerson = dto.ResponsiblePerson,
                CreatedAt = DateTime.UtcNow,
                RequestStatusId = existCategory.Id,
                Date = DateTime.TryParse(dto.Date, out var parsedDate) ? parsedDate.ToString("yyyy.MM.dd") : DateTime.UtcNow.ToString("yyyy.MM.dd"),
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


            var existCategory = await requestStatusRepository.GetAll(x => x.Id == dto.RequestStatusId && x.IsDeleted == 0).FirstOrDefaultAsync();

            existRequest.Client = dto.Client;
            existRequest.ClientCompany = dto.ClientCompany;
            existRequest.ContactNumber = dto.ContactNumber;
            existRequest.CompanyName = existCategory.Title;
            existRequest.Department = dto.Department;
            existRequest.Email = dto.Email;
            existRequest.InquiryField = dto.InquiryField;
            existRequest.InquiryType = dto.InquiryType;
            existRequest.ProcessingStatusId = dto.ProcessingStatus;
            existRequest.Notes = dto.Notes;
            existRequest.ProjectDetails = dto.ProjectDetails;
            existRequest.ResponsiblePerson = dto.ResponsiblePerson;
            existRequest.Date = DateTime.TryParse(dto.Date, out var parsedDate) ? parsedDate.ToString("yyyy.MM.dd") : DateTime.UtcNow.ToString("yyyy.MM.dd");
            existRequest.Status = dto.Status;
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
                var query = "SELECT * FROM \"Requests\" r LEFT JOIN \"ProcessingStatus\" rs ON r.\"ProcessingStatusId\" = rs.\"Id\" WHERE r.\"IsDeleted\" = @IsDeleted";
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
                    { "Date", x => DateTime.TryParse(x.Date, out var parsedDate) ? parsedDate.ToString("yyyy.MM.dd") : "" },
                    { "Department", x => x.Department },
                    { "Email", x => x.Email },
                    { "InquiryField", x => x.InquiryField },
                    { "InquiryType", x => x.InquiryType },
                    { "ProcessingStatus", x => x.ProcessingStatus == null ? null : x.ProcessingStatus.Text },
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
            var allRequestsRaw = await requestRepository
               .GetAll(x => x.IsDeleted == 0 && x.RequestStatusId != null)
               .Include(x => x.RequestStatus)
               .ToListAsync();

            var allCategory = await requestStatusRepository
                .GetAll(x => x.IsDeleted == 0)
                .ToListAsync();

            var totalCount = allRequestsRaw.Count;

            var result = new List<RequestRateModel>();

            var grouped = allRequestsRaw
                .GroupBy(x => x.RequestStatusId)
                .ToDictionary(g => g.Key, g => g.Count());

            var percentList = new List<CategoryPercent>();

            double totalPercent = 0;

            foreach (var category in allCategory)
            {
                grouped.TryGetValue(category.Id, out var statusCount);
                var percent = totalCount > 0 ? ((double)statusCount / totalCount * 100) : 0;
                totalPercent += percent;
                percentList.Add(new CategoryPercent
                {
                    Title = category.Title,
                    Count = statusCount,
                    RawPercent = percent,
                    RoundedPercent = (int)Math.Floor(percent),
                    Fraction = percent - Math.Floor(percent)
                });
            }

            int currentTotalPercent = percentList.Sum(x => x.RoundedPercent);
            int diff = 100 - currentTotalPercent;

            foreach (var item in percentList.OrderByDescending(x => x.Fraction).Take(diff))
            {
                item.RoundedPercent += 1;
            }

            result.Add(new RequestRateModel().MapFromEntity("all_requests", 100, totalCount));

            foreach (var item in percentList)
            {
                result.Add(new RequestRateModel().MapFromEntity(item.Title, item.RoundedPercent, item.Count));
            }

            return result;
        }

        public async ValueTask<List<RequestCountByStatusModel>> GetStatusCounts()
        {
            var allRequestsRaw = await requestRepository.GetAll(x => x.IsDeleted == 0 && x.Status != null).Include(x => x.RequestStatus).ToListAsync();
            var allCategory = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var groupedRequests = allRequestsRaw
             .GroupBy(x => x.RequestStatusId)
             .ToDictionary(g => g.Key, g => g.ToList());

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

            foreach (var category in allCategory)
            {
                groupedRequests.TryGetValue(category.Id, out var requestList);
                requestList ??= new List<Domain.Entities.Requests.Request>();

                var model = new RequestCountByStatusModel
                {
                    CategoryText = category.Title,
                    Counts = new List<StatusCountItem>
                    {
                        new StatusCountItem { Status = "Failed", Count = requestList.Count(x => x.Status == "Failed") },
                        new StatusCountItem { Status = "Made", Count = requestList.Count(x => x.Status == "Made") },
                        new StatusCountItem { Status = "On-going", Count = requestList.Count(x => x.Status == "On-going") },
                        new StatusCountItem { Status = "On-hold", Count = requestList.Count(x => x.Status == "On-hold") },
                        new StatusCountItem { Status = "Dropped", Count = requestList.Count(x => x.Status == "Dropped") },
                    },
                    Total = requestList.Count
                };

                result.Add(model);
            }

            return result;
        }

        public async ValueTask<List<RequestCountByStatusModel>> GetProccesingStatusCounts()
        {
            var allRequestsRaw = await requestRepository.GetAll(x => x.IsDeleted == 0 && x.Status != null).Include(x => x.RequestStatus).Include(x => x.ProcessingStatus).ToListAsync();
            var allCategory = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var allProccesingStatus = await processingStatus.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var groupedRequests = allRequestsRaw
             .GroupBy(x => x.RequestStatusId)
             .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<RequestCountByStatusModel>();

            var allRequestsModel = new RequestCountByStatusModel
            {
                CategoryText = "all_requests",
                Counts = allProccesingStatus.Select(status => new StatusCountItem
                {
                    Status = status.Text,
                    Count = allRequestsRaw.Count(x => x.ProcessingStatus != null && x.ProcessingStatus.IsDeleted == 0 && x.ProcessingStatus.Id == status.Id)
                }).ToList(),
                Total = allRequestsRaw.Count(x => x.ProcessingStatus != null && x.ProcessingStatus.IsDeleted == 0)
            };

            result.Add(allRequestsModel);

            foreach (var category in allCategory)
            {
                groupedRequests.TryGetValue(category.Id, out var requestList);
                requestList ??= new List<Domain.Entities.Requests.Request>();

                var model = new RequestCountByStatusModel
                {
                    CategoryText = category.Title,
                    Counts = allProccesingStatus.Select(status => new StatusCountItem
                    {
                        Status = status.Text,
                        Count = allRequestsRaw.Count(x => x.ProcessingStatus != null && x.ProcessingStatus.IsDeleted == 0 && x.ProcessingStatus.Id == status.Id && x.RequestStatusId == category.Id)
                    }).ToList(),
                    Total = requestList.Count(x => x.ProcessingStatus != null && x.ProcessingStatus.IsDeleted == 0)
                };

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
        public async ValueTask<List<Dictionary<string, object>>> GetPieChartData(int? year, int? month)
        {
            var allRequests = await requestRepository
                .GetAll(x => x.IsDeleted == 0 && x.Status != null)
                .ToListAsync();

            if (year is not null)
            {
                allRequests = allRequests
                    .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Year == year)
                    .ToList();
            }

            if(month is not null)
            {
                allRequests = allRequests
                    .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Month == month)
                    .ToList();
            }

            var allCategory = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var result = new List<Dictionary<string, object>>();

            foreach (var item in allCategory)
            {
                var dict = new Dictionary<string, object>
                {
                    ["CategoryId"] = item.Id,
                    ["Category"] = item.Title,
                    ["Made"] = allRequests.Where(x => x.RequestStatusId == item.Id).Count(x => x.Status == "Made"),
                    ["Failed"] = allRequests.Where(x => x.RequestStatusId == item.Id).Count(x => x.Status == "Failed"),
                    ["On-going"] = allRequests.Where(x => x.RequestStatusId == item.Id).Count(x => x.Status == "On-going"),
                    ["On-Hold"] = allRequests.Where(x => x.RequestStatusId == item.Id).Count(x => x.Status == "On-hold"),
                    ["Dropped"] = allRequests.Where(x => x.RequestStatusId == item.Id).Count(x => x.Status == "Dropped")
                };

                result.Add(dict);
            }

            return result;
        }


        public async ValueTask<List<Dictionary<string, object>>> GetLineChartData(int? year, int? month)
        {
            var allRequests = await requestRepository
                .GetAll(x => x.IsDeleted == 0 && x.Status != null)
                .Include(x => x.RequestStatus).Include(x => x.ProcessingStatus)
                .ToListAsync();

            foreach (var request in allRequests)
            {
                if (request.ProcessingStatus != null && request.ProcessingStatus.IsDeleted == 1)
                {
                    request.ProcessingStatus = null;
                }
            }

            if (year is not null)
            {
                allRequests = allRequests
                .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Year == year)
                .ToList();
            }

            if (month is not null)
            {
                allRequests = allRequests
                    .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Month == month)
                    .ToList();
            }

            var allCategories = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var allStatus = await processingStatus.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var result = new List<Dictionary<string, object>>();

            foreach (var status in allStatus)
            {
                var dict = new Dictionary<string, object>
                {
                    ["name"] = status.Text,
                    ["color"] = status.Color
                };

                foreach (var category in allCategories)
                {
                    var count = allRequests.Where(x =>
                        x.ProcessingStatusId == status.Id &&
                        x.RequestStatusId == category.Id).Count();

                    dict[category.Title] = count;
                }

                result.Add(dict);
            }

            return result;
        }

        public async ValueTask<Dictionary<string, object>> GetLineByStatusChartData(int? year, string status)
        {
            var allRequests = await requestRepository
                .GetAll(x => x.IsDeleted == 0 && x.Status != null && x.Status == status)
                .Include(x => x.RequestStatus).Include(x => x.ProcessingStatus)
                .ToListAsync();

            foreach (var request in allRequests)
            {
                if (request.ProcessingStatus != null && request.ProcessingStatus.IsDeleted == 1)
                {
                    request.ProcessingStatus = null;
                }
            }

            if (year is not null)
            {
                allRequests = allRequests
                    .Where(x => DateTime.TryParse(x.Date, out var parsedDate) && parsedDate.Year == year)
                    .ToList();
            }

            var allCategories = await requestStatusRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();


            var result = new Dictionary<string, object>();
            var monthlyData = new List<Dictionary<string, object>>();

            for (int month = 1; month <= 12; month++)
            {
                var monthName = new DateTime(2000, month, 1).ToString("MMM", CultureInfo.InvariantCulture);
                var dict = new Dictionary<string, object>
                {
                    ["month"] = monthName
                };

                foreach (var category in allCategories)
                {
                    var count = allRequests.Count(x => x.RequestStatusId == category.Id &&
                        DateTime.TryParse(x.Date, out var parsedDate) &&
                        parsedDate.Month == month);

                    dict[category.Title] = count;
                }

                monthlyData.Add(dict);
            }
            result[status] = monthlyData;
            return result;
        }

        public async ValueTask<bool> HardDeleteDeletedRequest(List<int> ids)
        {
            if (ids is null || ids.Count == 0) return false;

            foreach (var item in ids)
            {
                await requestRepository.DeleteAsync(item);
            }

            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> SoftDeleteOpenRequest(List<int> ids)
        {
            if (ids is null || ids.Count == 0) return false;

            foreach (var item in ids)
            {
                var existRequest = await requestRepository.GetAsync(x => x.Id == item);
                existRequest.IsDeleted = 1;
                requestRepository.UpdateAsync(existRequest);
            }

            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> SoftRecoverOpenRequest(List<int> ids)
        {
            if (ids is null || ids.Count == 0) return false;

            foreach (var item in ids)
            {
                var existRequest = await requestRepository.GetAsync(x => x.Id == item);
                existRequest.IsDeleted = 0;
                requestRepository.UpdateAsync(existRequest);
            }

            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> UploadFile(CreateUploadData dto)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");
            Domain.Entities.Attachment.Attachment attachment = null;
            if(dto.File is null) attachment = null;
            else attachment = await attachmentService.UploadAsync(dto.File.ToAttachmentOrDefault());
            existRequest.FileId = attachment?.Id;
            requestRepository.UpdateAsync(existRequest);
            await requestRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<string>  GetUploadedFile(int id)
        {
            var existRequest = await requestRepository.GetAll(x => x.Id == id).Include(x => x.File).FirstOrDefaultAsync();
            if (existRequest is null) throw new ProjectManagementException(404, "request_not_found");
            return existRequest?.File?.Path == null ? null : existRequest?.File?.Path;
        }
    }

    public class CreateUploadData
    {
        public IFormFile File { get; set; }
        public int Id { get; set; }
    }

    public class CategoryPercent
    {
        public string Title { get; set; }
        public int Count { get; set; }
        public double RawPercent { get; set; }
        public int RoundedPercent { get; set; }
        public double Fraction { get; set; }
    }
}
