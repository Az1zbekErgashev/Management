using ProjectManagement.Domain.Enum;


namespace ProjectManagement.Service.DTOs.MultilingualText
{
    public class MultilingualTextForCreateDTO
    {
        public string? Key { get; set; }
        public string? TextKo { get; set; }
        public string? TextEn { get; set; }
        public SupportLanguage SupportLanguage { get; set; }
    }
}
