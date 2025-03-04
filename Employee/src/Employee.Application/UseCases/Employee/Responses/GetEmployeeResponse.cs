using Employee.Domain.Models;
using Mapster;

namespace Employee.Application.UseCases.Employee.Responses;

public class GetEmployeeResponse : IRegister
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }
    public string ImmediateSupervisor { get; set; }
    public int PositionId { get; set; }
    public string PositionName { get; set; }
    public DateTime BirthDate { get; set; }
    public bool OfLegalAge { get; set; }
    public IEnumerable<GetEmployeePhoneResponse> Phones { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EmployeeModel, GetEmployeeResponse>()
            .Map(dest => dest.Name, src => src.Name + " " + src.Surname)
            .Map(dest => dest.PositionName, src => src.Position.Name);
    }

    public class GetEmployeePhoneResponse : PhoneResponse
    {
    }
}