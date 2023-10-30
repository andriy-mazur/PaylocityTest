namespace Api.Configurations;

public class PaycheckConfiguration
{
    public decimal MonthlyBaseBonus { get; set; } = 1000;
    public decimal MonthlyDependentBonus { get; set; } = 600;
    public decimal TopSalaryLevel { get; set; } = 80000;
    public decimal AnnualTopSalaryBonus { get; set; } = 0.02m;
    public int SeniorDependentAge { get; set; } = 50;
    public decimal MonthlySeniorDependentBonus { get; set; } = 200;
    public int PaychecksPerYear { get; set; } = 26;
}
