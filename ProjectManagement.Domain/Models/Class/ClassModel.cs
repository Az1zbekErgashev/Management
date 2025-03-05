using ProjectManagement.Domain.Models.Student;

namespace ProjectManagement.Domain.Models.Class;
public class ClassModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StudentModel> Students { get; set; }
    public DateTime CreateDate { get; set; }

    public virtual ClassModel MapFromEntity(Entities.Classes.Class @class)
    {
        Id = @class.Id;
        Name = @class.Name;
        CreateDate = @class.CreateAt;
        Students = @class.Students is not null && @class.Students.Count > 0 ? @class.Students.Select(x => new StudentModel().MapFromEntity(x)).ToList() : new List<StudentModel>();
        return this;
    }
}
