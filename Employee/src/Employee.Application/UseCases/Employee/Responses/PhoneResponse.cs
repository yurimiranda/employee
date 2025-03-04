using Employee.Application.UseCases.Employee.Base;

namespace Employee.Application.UseCases.Employee.Responses;

public abstract class PhoneResponse : Phone
{
    public int Id { get; set; }
}