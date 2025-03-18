namespace ProjectManagement.Domain.Models.Request
{
    public class RequestFilterModel
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public virtual RequestFilterModel MapFromEntity(string value, string key)
        {
            Text = value;
            Value = key;
            return this;
        }
    }
}
