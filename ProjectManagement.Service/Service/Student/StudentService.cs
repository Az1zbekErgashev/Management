using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Models.Student;
using ProjectManagement.Service.DTOs.Student;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.IRepositories;
using ProjectManagement.Service.Interfaces.Student;

namespace ProjectManagement.Service.Service.Student
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Domain.Entities.Students.Student> studentRepository;
        private readonly IGenericRepository<Domain.Entities.Classes.Class> classRepository;

        public StudentService(IGenericRepository<Domain.Entities.Students.Student> studentRepository, IGenericRepository<Domain.Entities.Classes.Class> classRepository)
        {
            this.studentRepository = studentRepository;
            this.classRepository = classRepository;
        }

        public async ValueTask<StudentModel> CreateAsync(StudentForCreationDTO @dto)
        {
            var existClass = await classRepository.GetAsync(x => x.Id == @dto.ClassId);
            if(existClass == null) throw new ProjectManagementException(404, "class_not_found");

            var model = new Domain.Entities.Students.Student
            {
                BirthDate = DateTime.SpecifyKind((DateTime)@dto.BirthDate, DateTimeKind.Utc),
                ClassId = @dto.ClassId,
                CreateAt = DateTime.UtcNow,
                FullName = @dto.FullName
            };

            await studentRepository.CreateAsync(model);
            await studentRepository.SaveChangesAsync();
            return new StudentModel().MapFromEntity(model);
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var status = await studentRepository.DeleteAsync(id);
            if (!status) throw new ProjectManagementException(404, "student_not_found");

            await studentRepository.SaveChangesAsync();
            return status;
        }
        public async ValueTask<List<StudentModel>> GetAsync()
        {
            var studentList = await studentRepository.GetAll().Include(x => x.Class).ToListAsync();
            var studentModelList = studentList.Select(x => new StudentModel().MapFromEntity(x)).ToList();
            return studentModelList;
        }

        public async ValueTask<StudentModel> GetById(int id)
        {
            var student = await studentRepository.GetAsync(x => x.Id == id);
            if (student is null)
                throw new ProjectManagementException(404, "student_not_found");

            return new StudentModel().MapFromEntity(student);
        }

        public async ValueTask<StudentModel> UpdateAsync(int id, StudentForCreationDTO @dto)
        {
            var student = await studentRepository.GetAsync(x => x.Id == id);

            if (student is null)
                throw new ProjectManagementException(404, "class_not_found");

            student.FullName = !string.IsNullOrEmpty(@dto.FullName) ? @dto.FullName : student.FullName;
            student.BirthDate = (DateTime)(@dto.BirthDate is null ? student.BirthDate : DateTime.SpecifyKind((DateTime)@dto.BirthDate, DateTimeKind.Utc));
            student.ClassId = @dto.ClassId != 0 ? @dto.ClassId : student.ClassId;

            studentRepository.UpdateAsync(student);
            await studentRepository.SaveChangesAsync();

            return new StudentModel().MapFromEntity(student);
        }
    }
}
