using Employee.Application.UseCases.Employee.Responses;
using Employee.Domain.ResultPattern;

namespace Employee.Application.Interfaces.UseCases;

public interface IEmployeeRetrievalUseCase
{
    Task<Result<GetEmployeeResponse, Error>> GetEmployee(Guid id);
}