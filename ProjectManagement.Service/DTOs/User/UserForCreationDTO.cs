namespace ProjectManagement.Service.DTOs.UserForCreationDTO
{
    public class UserForCreationDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public List<int>? TeamId { get; set; }
    }
}
