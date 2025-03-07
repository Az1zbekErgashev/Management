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
        public int? TeamLeaderId { get; set; }
        public required int CompanyId { get; set; }
        public required Role Role { get; set; }
        public bool IsIndependent { get; set; } = false;
        public IFormFile? Image { get; set; }
        public int CountryId { get; set; }
    }
}
