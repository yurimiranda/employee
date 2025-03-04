using Employee.Domain.Models;
using Mapster;

namespace Employee.Application.UseCases.Employee.Responses;

public class GetEmployeesResponse : IRegister
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EmployeeModel, GetEmployeesResponse>()
            .Map(dest => dest.Name, src => src.Name + " " + src.Surname);
    }
}