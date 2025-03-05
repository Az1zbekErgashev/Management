namespace ProjectManagement.Service.DTOs.Student
{
    public class StudentForCreationDTO
    {
        public int ClassId { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
