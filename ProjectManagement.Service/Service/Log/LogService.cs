using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Models.Log;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Service.DTOs.Log;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Log;
using ProjectManagement.Service.StringExtensions;


namespace ProjectManagement.Service.Service.Log
{
    public class LogService : ILogService
    {
        private readonly IGenericRepository<Logs> _logRepository;

        public LogService(IGenericRepository<Logs> logRepository)
        {
            _logRepository = logRepository;
        }

        public async ValueTask<PagedResult<LogsModel>> GetAsync(LogsForFilterDTO dto)
        {
            var query = _logRepository.GetAll(x => x.IsDeleted == 0).Include(x => x.User).AsQueryable();

            query = query.OrderByDescending(x => x.Id);

            if(dto.Action is not null)
            {
                query = query.Where(x => x.Action == dto.Action);
            }

            if(dto.UserId is not null)
            {
                query = query.Where(x => x.UserId == dto.UserId);
            }

            if (dto.StartDate is not null)
            {
                var startDate = dto.StartDate.Value.Date; 
                query = query.Where(x => x.CreatedAt.Date >= startDate);
            }

            if (dto.EndDate is not null)
            {
                var endDate = dto.EndDate.Value.Date; 
                query = query.Where(x => x.CreatedAt.Date <= endDate);
            }

            if (!string.IsNullOrEmpty(dto.Text))
            {
                string searchText = $"%{dto.Text}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.User.Name, searchText) ||
                    EF.Functions.Like(x.User.Surname, searchText) ||
                    EF.Functions.Like(x.User.Email, searchText));
            }

            int totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                return PagedResult<LogsModel>.Create(
                    Enumerable.Empty<LogsModel>(),
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
            query = query.AsQueryable();

            var list = await query.ToListAsync();

            List<LogsModel> models = list.Select(
                f => new LogsModel().MapFromEntity(f))
                .ToList();


            var pagedResult = PagedResult<LogsModel>.Create(models,
                totalCount,
                itemsPerPage,
                models.Count,
                dto.PageIndex,
                totalPages
                );

            return pagedResult;
        }
    }
}
