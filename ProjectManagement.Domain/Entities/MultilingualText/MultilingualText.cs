using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.MultilingualText
{
    public class MultilingualText : Auditable
    {
        public required string Key { get; set; }
        public required string TextKo { get; set; }
        public required string TextEn { get; set; }
        public SupportLanguage SupportLanguage { get; set; }
    }
}
