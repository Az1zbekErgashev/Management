using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;


namespace ProjectManagement.Service.Interfaces.MultilingualText
{
    public interface IMultilingualTextInterface
    {
        ValueTask<bool> CreateFromJson(IFormFile formFile, SupportLanguage language);
        ValueTask<Dictionary<string, string>> GetDictonary(SupportLanguage language);
    }
}
