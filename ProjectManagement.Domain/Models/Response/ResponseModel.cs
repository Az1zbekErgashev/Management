
namespace ProjectManagement.Domain.Models.Response
{
    public class ResponseModel<T>
    {
        public bool Status { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
        public bool? Global { get; set; } = null;

    }

    public class ResponseModel
    {
        public static ResponseModel<U> Create<U>(U model, bool status, bool global, string error)
        {
            return new ResponseModel<U> { Data = model, Status = status, Error = error, Global = global };
        }
    }
}
