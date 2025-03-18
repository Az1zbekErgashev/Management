namespace ProjectManagement.Service.DTOs.User
{
    public class UserForLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
