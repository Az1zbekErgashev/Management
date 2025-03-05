namespace ProjectManagement.Service.Exception;

public class ProjectManagementException : System.Exception
{
    public int Code { get; set; }
    public bool? Global { get; set; }

    public ProjectManagementException(int code, string message, bool? global = true) : base(message)
    {
        Code = code;
        Global = global;
    }
}
