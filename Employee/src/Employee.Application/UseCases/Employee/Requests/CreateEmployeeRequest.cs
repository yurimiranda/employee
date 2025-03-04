using Employee.Application.UseCases.Employee.Base;
using Employee.Domain.Enums;

namespace Employee.Application.UseCases.Employee.Requests;

public class CreateEmployeeRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionId { get; set; }
    public Role Role { get; set; }
    public DateTime BirthDate { get; set; }
    public IEnumerable<CreatePhoneRequest> Phones { get; set; }
    public string Password { get; set; }

    public class CreatePhoneRequest : Phone
    {
    }
}