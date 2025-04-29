namespace ProjectManagement.Domain.Models.Request
{
    public class RequestRateModel
    {
        public string CategoryText { get; set; }
        public int Procent { get; set; }
        public int Total { get; set; }

        public virtual RequestRateModel MapFromEntity(string text, int procent, int total)
        {
            CategoryText = text;
            Procent = procent;
            Total = total;
            return this;
        }
    }
}
