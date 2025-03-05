namespace Employee.Application.UseCases.User.Requests;

public class UserSignInRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}