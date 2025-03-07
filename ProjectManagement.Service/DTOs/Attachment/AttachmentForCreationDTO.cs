using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Service.DTOs.Attachment
{
    public class AttachmentForCreationDTO
    {
        [Required]
        public string Path { get; set; }

        [Required]
        public Stream Stream { get; set; }
    }
}
