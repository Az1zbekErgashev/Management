namespace ProjectManagement.Service.DTOs.Request
{
    public class ForCreateManyRequest
    {
        public ICollection<RequestForCreateDTO> Dto { get; set; }
        public int RequestStatusId { get; set; }
    }
}
