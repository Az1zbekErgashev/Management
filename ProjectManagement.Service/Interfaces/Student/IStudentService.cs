using ProjectManagement.Domain.Models.Student;
using ProjectManagement.Service.DTOs.Student;


namespace ProjectManagement.Service.Interfaces.Student;
public interface IStudentService
{
    ValueTask<bool> DeleteAsync(int id);
    ValueTask<List<StudentModel>> GetAsync();
    ValueTask<StudentModel> GetById(int id);
    ValueTask<StudentModel> CreateAsync(StudentForCreationDTO @dto);
    ValueTask<StudentModel> UpdateAsync(int id, StudentForCreationDTO @dto);
}
