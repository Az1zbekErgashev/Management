using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Request;

namespace ProjectManagement.Service.Service.Request
{
    public class RequestStatusService : IRequestStatusService
    {
        private readonly IGenericRepository<RequestStatus> requestStatusRepository;

        public RequestStatusService(IGenericRepository<RequestStatus> requestStatusRepository)
        {
            this.requestStatusRepository = requestStatusRepository;
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
    }
}
