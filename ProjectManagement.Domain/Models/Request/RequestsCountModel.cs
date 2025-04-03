namespace ProjectManagement.Domain.Models.Request
{
    public class RequestsCountModel
    {
        public string Title { get; set; }
        public int Count { get; set; }


        public virtual RequestsCountModel MapFromEntity(string title, int count)
        {
            Title = title;
            Count = count;
            return this;
        }
    }
}
