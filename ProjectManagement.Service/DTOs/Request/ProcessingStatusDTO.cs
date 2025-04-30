
namespace ProjectManagement.Service.DTOs.Request
{
    public class ProcessingStatusDTO
    {
        public int? Id { get; set; }
        public required string Text { get; set; }
        public required string Color { get; set; }
    }
}
