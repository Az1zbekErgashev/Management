using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.MultilingualText
{
    public class MultilingualText : Auditable
    {
        public string? Key { get; set; }
        public string? Text { get; set; }
        public SupportLanguage SupportLanguage { get; set; }

    }
}
