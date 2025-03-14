using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Service.DTOs.User
{
    public class UserForUpdateDTO
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public  string? Password { get; set; }
        public required string Surname { get; set; }
        public required string PhoneNumber { get; set; }

        [Required]
        public int UserId { get; set; }
        public required Role Role { get; set; }
        public IFormFile? Image { get; set; }
        public bool UpdateImage { get; set; }
        public int CountryId { get; set; }
    }
}
