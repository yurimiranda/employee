namespace Employee.Application.UseCases.Employee.Requests;

public class GetEmployeesRequest
{
    public string Email { get; set; }
    public string Document { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}