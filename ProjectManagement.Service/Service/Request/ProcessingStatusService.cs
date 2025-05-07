
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;
using ProjectManagement.Service.StringExtensions;

namespace ProjectManagement.Service.Service.Request
{
    public class ProcessingStatusService : IProcessingStatusService
    {
        private readonly IGenericRepository<ProcessingStatus> processingStatusRepository;
        private readonly IGenericRepository<Domain.Entities.Requests.Request> requestsRepository;

        public ProcessingStatusService(IGenericRepository<ProcessingStatus> processingStatusRepository, IGenericRepository<Domain.Entities.Requests.Request> requestsRepository)
        {
            this.processingStatusRepository = processingStatusRepository;
            this.requestsRepository = requestsRepository;
        }

        public async ValueTask<bool> CreateAsync(ProcessingStatusDTO dto)
        {
            var status = new ProcessingStatus
            {
                Color = dto.Color,
                Text = dto.Text
            };

            await processingStatusRepository.CreateAsync(status);
            await processingStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var existStatus = await processingStatusRepository.GetAsync(x => x.Id == id);
            if (existStatus == null) throw new ProjectManagementException(404, "status_not_found");

            var allRequests = await requestsRepository.GetAll(x => x.ProcessingStatusId == existStatus.Id).ToListAsync();

            foreach (var item in allRequests)
            {
                item.ProcessingStatusId = null;
                requestsRepository.UpdateAsync(item);
            }

            await requestsRepository.SaveChangesAsync();

            await processingStatusRepository.DeleteAsync(existStatus.Id);
            await processingStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<PagedResult<ProcessingStatusModel>> GetAllAsync(ProcessingStatusFilter dto)
        {
            var query = processingStatusRepository.GetAll(null)
               .AsQueryable();

            query = query.OrderBy(x => x.Id);

            int totalCount = await query.CountAsync();

            if (totalCount == 0)
            {
                return PagedResult<ProcessingStatusModel>.Create(
                    Enumerable.Empty<ProcessingStatusModel>(),
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

            List<ProcessingStatusModel> models = list.Select(
                f => new ProcessingStatusModel().MapFromEntity(f))
                .ToList();


            var pagedResult = PagedResult<ProcessingStatusModel>.Create(models,
                totalCount,
                itemsPerPage,
                models.Count,
                dto.PageIndex,
                totalPages
                );

            return pagedResult;
        }

        public async ValueTask<ProcessingStatusModel> GetByIdAsync(int id)
        {
            var existStatus = await processingStatusRepository.GetAsync(x => x.Id == id);
            if (existStatus == null) throw new ProjectManagementException(404, "status_not_found");

            return new ProcessingStatusModel().MapFromEntity(existStatus);
        }

        public async ValueTask<bool> UpdateAsync(ProcessingStatusDTO dto)
        {
            if (dto.Id == null) throw new ProjectManagementException(404, "status_not_found");
            var existStatus = await processingStatusRepository.GetAsync(x => x.Id == dto.Id);
            if (existStatus == null) throw new ProjectManagementException(404, "status_not_found");

            existStatus.Color = dto.Color;
            existStatus.Text = dto.Text;

            processingStatusRepository.UpdateAsync(existStatus);
            await processingStatusRepository.SaveChangesAsync();
            return true;
        }

        public async ValueTask<bool> DeleteListAsync(List<int> ints)
        {
            if (ints == null || ints.Count == 0) return false;

            foreach (var item in ints)
            {
                var existStatus = await processingStatusRepository.DeleteAsync(item);
            }

            await processingStatusRepository.SaveChangesAsync();
            return true;
        }
    }

    public class ProcessingStatusFilter : PaginationParams
    {
    }


}
