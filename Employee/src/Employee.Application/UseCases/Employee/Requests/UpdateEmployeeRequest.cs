using Employee.Application.UseCases.Employee.Base;
using Employee.Domain.Enums;

namespace Employee.Application.UseCases.Employee.Requests;

public class UpdateEmployeeRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionId { get; set; }
    public Role Role { get; set; }
    public IEnumerable<UpdatePhoneRequest> Phones { get; set; }

    public class UpdatePhoneRequest : Phone
    {
        public int Id { get; set; }
    }
}