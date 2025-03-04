namespace Employee.Application.UseCases.Employee.Responses;

public class DeleteEmployeeResponse(Guid id)
{
    public Guid Id { get; set; } = id;
}