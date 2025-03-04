using Employee.Application.UseCases.Employee.Base;

namespace Employee.Application.UseCases.Employee.Requests;

public class UpdateEmployeeRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionRoleId { get; set; }
    public IEnumerable<UpdatePhoneRequest> Phones { get; set; }

    public class UpdatePhoneRequest : Phone
    {
        public int Id { get; set; }
    }
}