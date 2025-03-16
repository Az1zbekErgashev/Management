using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Service.DTOs.UserForCreationDTO
{
    public class UserForCreationDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }
        public required Role Role { get; set; }
        public IFormFile? Image { get; set; }
        public int CountryId { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
