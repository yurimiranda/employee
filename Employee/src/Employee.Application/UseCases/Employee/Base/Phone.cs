namespace Employee.Application.UseCases.Employee.Base;

public abstract class Phone
{
    public string AreaCode { get; set; }
    public string Number { get; set; }
    public bool IsPrimary { get; set; }
}