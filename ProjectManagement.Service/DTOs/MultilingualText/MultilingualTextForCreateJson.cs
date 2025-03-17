using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Service.DTOs.MultilingualText
{
    public class MultilingualTextForCreateJson
    {
        public IFormFile File { get; set; }
        public SupportLanguage Language { get; set; }
    }
}
