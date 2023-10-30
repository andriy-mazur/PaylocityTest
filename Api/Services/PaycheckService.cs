using Api.Configurations;
using Api.Dtos.Employee;
using Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Api.Services;

public class PaycheckService : IPaycheckService
{
    private readonly IEmployeeService _employeeService;
    private readonly PaycheckConfiguration _paycheckConfiguration;

    public PaycheckService(IEmployeeService employeeService, IOptions<PaycheckConfiguration> paycheckConfiguration)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _paycheckConfiguration = paycheckConfiguration.Value ??
                                 throw new ArgumentNullException(nameof(paycheckConfiguration));
    }

    /// <summary>
    /// Calculate paycheck details.
    /// Paycheck is calculating on a specific date because
    /// - The last paycheck in a year should adjust the rounding from the previous paychecks
    /// - Senior Dependants Bonus need a date to calculate the dependant Age
    /// Some bonuses has monthly amount some others has annual amount.
    /// To pay by paycheck period the annual amount is calculated and divided by paychecks count.
    /// </summary>
    /// <param name="employeeId">Employee ID to calculate paycheck</param>
    /// <param name="onDate">Calculate paycheck on a specific date</param>
    /// <returns>Paycheck details</returns>
    public async Task<GetPaycheckDto?> GetPaycheckAsync(int employeeId, DateTime onDate)
    {
        var employee = await _employeeService.GetAsync(employeeId);

        if (employee == null)
            throw new Exception($"Employee not found id={employeeId}");

        var result = new GetPaycheckDto
        {
            AnnualSalary = employee.Salary,
            AnnualBaseBonus = GetAnnualBaseBonus(),
            AnnualDependentsBonus = GetAnnualDependentsBonus(employee),
            AnnualTopSalaryBonus = GetAnnualTopSalaryBonus(employee),
            AnnualSeniorDependentsBonus = GetAnnualSeniorDependentsBonus(employee, onDate)
        };

        var isLastPayCheckInYear = IsLastPayCheckInYear(onDate);

        result.PaycheckAmount = GetPaycheckAmount(result.AnnualSalary, isLastPayCheckInYear);
        result.PaycheckBaseBonus = GetPaycheckAmount(result.AnnualBaseBonus, isLastPayCheckInYear);
        result.PaycheckDependentsBonus = GetPaycheckAmount(result.AnnualDependentsBonus, isLastPayCheckInYear);
        result.PaycheckTopSalaryBonus = GetPaycheckAmount(result.AnnualTopSalaryBonus, isLastPayCheckInYear);
        result.PaycheckSeniorDependentsBonus = GetPaycheckAmount(
            result.AnnualSeniorDependentsBonus,
            isLastPayCheckInYear
        );

        result.TotalPaycheckAmount = result.PaycheckAmount +
                                     result.PaycheckBaseBonus +
                                     result.PaycheckDependentsBonus +
                                     result.PaycheckTopSalaryBonus +
                                     result.PaycheckSeniorDependentsBonus;

        return result;
    }

    private bool IsLastPayCheckInYear(DateTime onDate)
    {
        var daysInYear = 365 + (DateTime.IsLeapYear(onDate.Year) ? 1 : 0);
        var payPeriod = (int)Math.Floor(
            1m + 1m * onDate.DayOfYear / daysInYear * _paycheckConfiguration.PaychecksPerYear
        );

        return payPeriod == _paycheckConfiguration.PaychecksPerYear;
    }

    private decimal GetAnnualBaseBonus()
    {
        return _paycheckConfiguration.MonthlyBaseBonus * 12;
    }

    /// <summary>
    /// Get Dependants bonus
    /// </summary>
    /// <param name="employee">Employee to calc</param>
    /// <returns>Dependants bonus</returns>
    private decimal GetAnnualDependentsBonus(GetEmployeeDto employee)
    {
        return employee.Dependents.Count * _paycheckConfiguration.MonthlyDependentBonus * 12;
    }

    /// <summary>
    /// Get bonus for top salary employees.
    /// Employee should have a salary GREATER then specific to achieve a bonus
    /// </summary>
    /// <param name="employee">Employee to calc</param>
    /// <returns>Top Salary Bonus</returns>
    private decimal GetAnnualTopSalaryBonus(GetEmployeeDto employee)
    {
        return employee.Salary <= _paycheckConfiguration.TopSalaryLevel
            ? 0
            : Math.Round(employee.Salary * _paycheckConfiguration.AnnualTopSalaryBonus, 2);
    }

    /// <summary>
    /// Get Senior Dependants bonus.
    /// Age compared with >= because if person has the same age as
    /// </summary>
    /// <param name="employee">Employee to calc</param>
    /// <param name="onDate">Calculate Dependants Age on specific date</param>
    /// <returns>Senior Dependents Bonus</returns>
    private decimal GetAnnualSeniorDependentsBonus(GetEmployeeDto employee, DateTime onDate)
    {
        return employee.Dependents.Count(
                   d => GetAge(d.DateOfBirth, onDate) >= _paycheckConfiguration.SeniorDependentAge
               ) *
               _paycheckConfiguration.MonthlySeniorDependentBonus *
               12;
    }

    /// <summary>
    /// Get a person age by DateOfBirth on a first day of a year.
    /// Based on the requirements the Dependant Age can be calculated:
    /// - on a first day of the year
    /// - on a last day of the year
    /// - for the each paycheck (Dependent can become Senior during the year)
    /// </summary>
    /// <param name="dateOfBirth">Date of Birth</param>
    /// <param name="onDate">Calculate Dependants Age on specific date</param>
    /// <returns>Age</returns>
    private static int GetAge(DateTime dateOfBirth, DateTime onDate)
    {
        var calcDate = new DateTime(onDate.Year, 1, 1);

        var calcDateNumber = (calcDate.Year * 100 + calcDate.Month) * 100 + calcDate.Day;
        var dobNumber = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

        return (calcDateNumber - dobNumber) / 10000;
    }

    /// <summary>
    /// Calculate the proper paycheck field amount.
    /// The last paycheck amount in a year should be adjusted to consider the rounding of the previous paychecks
    /// </summary>
    /// <param name="annualAmount">Annual payment amount</param>
    /// <param name="isLastPayCheckInYear">Is it last paycheck in a year</param>
    /// <returns>Adjusted paycheck amount</returns>
    private decimal GetPaycheckAmount(decimal annualAmount, bool isLastPayCheckInYear)
    {
        var regularPaycheck = Math.Round(annualAmount / _paycheckConfiguration.PaychecksPerYear, 2);

        return isLastPayCheckInYear
            ? annualAmount - (_paycheckConfiguration.PaychecksPerYear - 1) * regularPaycheck
            : regularPaycheck;
    }
}
