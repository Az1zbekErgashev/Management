namespace ProjectManagement.Domain.Models.Request
{
    public class RequestCountByStatusModel
    {
        public string CategoryText { get; set; }
        public List<StatusCountItem> Counts { get; set; } = new();
        public int Total { get; set; }

        public virtual RequestCountByStatusModel MapFromEntity(List<Entities.Requests.Request> entity)
        {
            CategoryText = entity.FirstOrDefault().RequestStatus.Title;
            Counts = new List<StatusCountItem>
            {
                new StatusCountItem { Status = "Failed", Count = entity.Count(x => x.Status == "Failed") },
                new StatusCountItem { Status = "Made", Count = entity.Count(x => x.Status == "Made") },
                new StatusCountItem { Status = "On-going", Count = entity.Count(x => x.Status == "On-going") },
                new StatusCountItem { Status = "On-hold", Count = entity.Count(x => x.Status == "On-hold") },
                new StatusCountItem { Status = "Dropped", Count = entity.Count(x => x.Status == "Dropped") }
            };
            Total = entity.Count();
            return this;
        }
    }

    public class StatusCountItem
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
