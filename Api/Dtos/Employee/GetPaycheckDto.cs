namespace Api.Dtos.Employee;

public class GetPaycheckDto
{
    public decimal AnnualSalary { get; set; }
    public decimal AnnualBaseBonus { get; set; }
    public decimal AnnualDependentsBonus { get; set; }
    public decimal AnnualTopSalaryBonus { get; set; }
    public decimal AnnualSeniorDependentsBonus { get; set; }
    public decimal PaycheckAmount { get; set; }
    public decimal PaycheckBaseBonus { get; set; }
    public decimal PaycheckDependentsBonus { get; set; }
    public decimal PaycheckTopSalaryBonus { get; set; }
    public decimal PaycheckSeniorDependentsBonus { get; set; }
    public decimal TotalPaycheckAmount { get; set; }
}
