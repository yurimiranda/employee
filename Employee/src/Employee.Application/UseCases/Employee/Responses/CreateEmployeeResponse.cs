namespace Employee.Application.UseCases.Employee.Responses;

public class CreateEmployeeResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}