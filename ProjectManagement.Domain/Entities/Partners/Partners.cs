using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Partners;
public class Partners : Auditable
{
    public required string Name { get; set; }
    public required string CompanyCode { get; set; }
    public required string CompanyLocation { get; set; }
    public virtual Attachment.Attachment? Image { get; set; }
    public int? ImageId { get; set; }
};
