using ProjectManagement.Domain.Enum;


namespace ProjectManagement.Domain.Models.MultilingualText
{
    public class MultilingualTextModel
    {
        public string? Key { get; set; }
        public string? Text { get; set; }
        public SupportLanguage SupportLanguage { get; set; }
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public MultilingualTextModel MapFromEntity(Domain.Entities.MultilingualText.MultilingualText entity)
        {
            Id = entity.Id;
            UpdatedAt = entity.UpdatedAt;
            CreatedAt = entity.CreatedAt;
            Key = entity.Key;
            Text = entity.Text;
            SupportLanguage = entity.SupportLanguage;
            return this;
        }
    }
}
