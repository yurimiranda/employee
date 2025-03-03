namespace Employee.Application.UseCases.Employee.Requests;

public class CreateEmployeeRequest
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionRoleId { get; set; }
    public DateTime BirthDate { get; set; }
    public IEnumerable<CreatePhoneRequest> Phones { get; set; }

    public class CreatePhoneRequest
    {
        public string AreaCode { get; set; }
        public string Number { get; set; }
        public bool IsPrimary { get; set; }
    }
}