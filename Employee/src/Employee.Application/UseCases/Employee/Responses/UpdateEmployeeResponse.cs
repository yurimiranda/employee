using Employee.Application.UseCases.Employee.Base;

namespace Employee.Application.UseCases.Employee.Responses;

public class UpdateEmployeeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionRoleId { get; set; }
    public IEnumerable<UpdatePhoneResponse> Phones { get; set; }

    public class UpdatePhoneResponse : Phone
    {
    }
}