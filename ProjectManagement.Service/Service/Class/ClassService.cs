using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Models.Class;
using ProjectManagement.Service.DTOs.Class;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.Class;
using ProjectManagement.Service.Interfaces.IRepositories;

namespace ProjectManagement.Service.Service.Class
{
    public class ClassService : IClassService
    {
        private readonly IGenericRepository<Domain.Entities.Classes.Class> classRepository;
        private readonly IGenericRepository<Domain.Entities.Students.Student> studentRepository;

        public ClassService(IGenericRepository<Domain.Entities.Classes.Class> classRepository,
            IGenericRepository<Domain.Entities.Students.Student> studentRepository)
        {
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
        }

        public async ValueTask<ClassModel> CreateAsync(ClassForCreationDTO @dto)
        {
            var model = new Domain.Entities.Classes.Class
            {
                CreateAt = DateTime.UtcNow,
                Name = @dto.Name
            };

            await classRepository.CreateAsync(model);
            await classRepository.SaveChangesAsync();
            return new ClassModel().MapFromEntity(model);
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var status = await classRepository.DeleteAsync(id);
            if (!status) throw new ProjectManagementException(404, "class_not_found");

            await classRepository.SaveChangesAsync();
            return status;
        }

        public async ValueTask<List<ClassModel>> GetAsync()
        {
            var classList = await classRepository.GetAll().Include(x => x.Students).ToListAsync();
            var classModelList = classList.Select(x => new ClassModel().MapFromEntity(x)).ToList();
            return classModelList;
        }

        public async ValueTask<ClassModel> GetById(int id)
        {
            var classModel = await classRepository.GetAsync(x => x.Id == id);
            if (classModel is null)
                throw new ProjectManagementException(404, "class_not_found");

            return new ClassModel().MapFromEntity(classModel);
        }

        public async ValueTask<ClassModel> UpdateAsync(int id, ClassForCreationDTO dto)
        {
            var classModel = await classRepository.GetAsync(x => x.Id == id);

            if (classModel is null)
                throw new ProjectManagementException(404, "class_not_found");

            classModel.Name = !string.IsNullOrEmpty(@dto.Name) ? @dto.Name : classModel.Name;

            classRepository.UpdateAsync(classModel);
            await classRepository.SaveChangesAsync();

            return new ClassModel().MapFromEntity(classModel);
        }
    }
}
