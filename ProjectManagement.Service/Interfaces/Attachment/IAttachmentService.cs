using ProjectManagement.Service.DTOs.Attachment;

namespace ProjectManagement.Service.Interfaces.Attachment
{
    public interface IAttachmentService
    {
        ValueTask<Domain.Entities.Attachment.Attachment> UploadAsync(AttachmentForCreationDTO dto);
        ValueTask<Domain.Entities.Attachment.Attachment> UpdateAsync(int id, Stream stream);
        ValueTask<bool> ResizeImage(Domain.Entities.User.User user, int dimension);
    }
}
