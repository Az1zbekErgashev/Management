using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Service.DTOs.Partner
{
    public class PartnerForCreateDTO
    {
        public required string Name { get; set; }
        public required string CompanyCode { get; set; }
        public int CountryId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Site { get; set; }
        public string? EmployeesCount { get; set; }
        public string? Description { get; set; }
    }
}
