namespace Employee.Domain.ResultPattern;

public class Error
{
    public string Code { get; }
    public IEnumerable<string> ErrorMessages { get; }

    private Error(string code, IEnumerable<string> errorMessages)
    {
        Code = code;
        ErrorMessages = errorMessages;
    }

    public static Error Throw(string code, IEnumerable<string> errorMessages)
        => new(code, errorMessages);

    public static Error Throw(string code, string message)
        => Throw(code, [message]);
}