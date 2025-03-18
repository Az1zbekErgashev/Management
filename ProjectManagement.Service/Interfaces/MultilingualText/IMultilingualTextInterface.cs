using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.MultilingualText;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Service.DTOs.MultilingualText;


namespace ProjectManagement.Service.Interfaces.MultilingualText
{
    public interface IMultilingualTextInterface
    {
        ValueTask<bool> CreateFromJson(IFormFile formFile, SupportLanguage language);
        ValueTask<Dictionary<string, string>> GetDictonary(SupportLanguage language);
        ValueTask<PagedResult<UIContentExtendedModel>> GetTranslations(UIContentGetAllAndSearchDTO dto);
        ValueTask<bool> UpdateAsync(MultilingualTextForCreateDTO dto);
        ValueTask<bool> CreateAsync(MultilingualTextForCreateDTO dto);
        ValueTask<string> DeleteOrRecoverAsync(string key);
    }
}
