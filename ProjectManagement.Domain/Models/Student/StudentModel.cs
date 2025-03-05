namespace ProjectManagement.Domain.Models.Student;

public class StudentModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime BirthDate { get; set; }
    public int ClassId { get; set; }

    public virtual StudentModel MapFromEntity(Entities.Students.Student @student)
    {
        Id = @student.Id;
        FullName = @student.FullName;
        CreatedDate = @student.CreateAt;
        BirthDate = @student.BirthDate;
        ClassId = @student.ClassId;
        return this;
    }
}
