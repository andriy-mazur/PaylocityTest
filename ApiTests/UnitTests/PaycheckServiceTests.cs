using System;
using System.Threading.Tasks;
using Api.Configurations;
using Api.Dtos.Employee;
using Api.Services;
using Api.Services.Interfaces;
using ApiTests.UnitTests.Fakers;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ApiTests.UnitTests;

public class PaycheckServiceTests
{
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly GetEmployeeDto _employee;

    private readonly PaycheckService _service;

    public PaycheckServiceTests()
    {
        _employee = new GetEmployeeDtoFaker().Generate();
        _employee.Salary = 52000;

        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeServiceMock.Setup(m => m.GetAsync(It.IsAny<int>())).ReturnsAsync(_employee);

        var paycheckConfiguration = new PaycheckConfiguration
        {
            MonthlyBaseBonus = 1000,
            MonthlyDependentBonus = 600,
            TopSalaryLevel = 80000,
            AnnualTopSalaryBonus = 0.02m,
            SeniorDependentAge = 50,
            MonthlySeniorDependentBonus = 200,
            PaychecksPerYear = 26
        };

        var paycheckConfigurationOptions = Options.Create(paycheckConfiguration);

        _service = new PaycheckService(_employeeServiceMock.Object, paycheckConfigurationOptions);
    }

    [Fact]
    public async Task GetPaycheckAsync_Success()
    {
        var onDate = new DateTime(DateTime.Now.Year, 5, 5);
        var paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(_employee.Salary, paycheck!.AnnualSalary);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(0, paycheck.AnnualDependentsBonus);
        Assert.Equal(0, paycheck.AnnualTopSalaryBonus);
        Assert.Equal(0, paycheck.AnnualSeniorDependentsBonus);
        Assert.Equal(2000, paycheck.PaycheckAmount);
        Assert.Equal(461.54m, paycheck.PaycheckBaseBonus);
        Assert.Equal(0, paycheck.PaycheckDependentsBonus);
        Assert.Equal(0, paycheck.PaycheckTopSalaryBonus);
        Assert.Equal(0, paycheck.PaycheckSeniorDependentsBonus);
        Assert.Equal(2461.54m, paycheck.TotalPaycheckAmount);
    }

    [Fact]
    public async Task GetPaycheckAsync_ShouldThrowIfEmployeeNotFound()
    {
        _employeeServiceMock.Setup(m => m.GetAsync(It.IsAny<int>())).ReturnsAsync((GetEmployeeDto)null!);

        var onDate = new DateTime(DateTime.Now.Year, 5, 5);
        var exception = await Assert.ThrowsAsync<Exception>(() => _service.GetPaycheckAsync(_employee.Id, onDate));

        Assert.NotNull(exception);
        Assert.Equal($"Employee not found id={_employee.Id}", exception.Message);
    }

    [Fact]
    public async Task GetPaycheckAsync_ShouldCalculateDependantsBonus()
    {
        _employee.Dependents = new GetDependentDtoFaker().Generate(3);

        var onDate = new DateTime(DateTime.Now.Year, 5, 5);
        var paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(_employee.Salary, paycheck!.AnnualSalary);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(21600, paycheck.AnnualDependentsBonus);
        Assert.Equal(0, paycheck.AnnualTopSalaryBonus);
        Assert.Equal(0, paycheck.AnnualSeniorDependentsBonus);
        Assert.Equal(2000, paycheck.PaycheckAmount);
        Assert.Equal(461.54m, paycheck.PaycheckBaseBonus);
        Assert.Equal(830.77m, paycheck.PaycheckDependentsBonus);
        Assert.Equal(0, paycheck.PaycheckTopSalaryBonus);
        Assert.Equal(0, paycheck.PaycheckSeniorDependentsBonus);
        Assert.Equal(3292.31m, paycheck.TotalPaycheckAmount);
    }

    [Fact]
    public async Task GetPaycheckAsync_ShouldCalculateTopSalaryBonus()
    {
        _employee.Salary = 104000;

        var onDate = new DateTime(DateTime.Now.Year, 5, 5);
        var paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(_employee.Salary, paycheck!.AnnualSalary);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(0, paycheck.AnnualDependentsBonus);
        Assert.Equal(2080, paycheck.AnnualTopSalaryBonus);
        Assert.Equal(0, paycheck.AnnualSeniorDependentsBonus);
        Assert.Equal(4000, paycheck.PaycheckAmount);
        Assert.Equal(461.54m, paycheck.PaycheckBaseBonus);
        Assert.Equal(0, paycheck.PaycheckDependentsBonus);
        Assert.Equal(80, paycheck.PaycheckTopSalaryBonus);
        Assert.Equal(0, paycheck.PaycheckSeniorDependentsBonus);
        Assert.Equal(4541.54m, paycheck.TotalPaycheckAmount);
    }

    [Fact]
    public async Task GetPaycheckAsync_ShouldCalculateSeniorDependentsBonus()
    {
        // Add Senior Dependant
        _employee.Dependents = new GetDependentDtoFaker()
            .FinishWith((_, d) => d.DateOfBirth = new DateTime(DateTime.Now.Year - 60, 2, 2))
            .Generate(1);

        var onDate = new DateTime(DateTime.Now.Year, 5, 5);
        var paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(_employee.Salary, paycheck!.AnnualSalary);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(7200, paycheck.AnnualDependentsBonus);
        Assert.Equal(0, paycheck.AnnualTopSalaryBonus);
        Assert.Equal(2400, paycheck.AnnualSeniorDependentsBonus);
        Assert.Equal(2000, paycheck.PaycheckAmount);
        Assert.Equal(461.54m, paycheck.PaycheckBaseBonus);
        Assert.Equal(276.92m, paycheck.PaycheckDependentsBonus);
        Assert.Equal(0, paycheck.PaycheckTopSalaryBonus);
        Assert.Equal(92.31m, paycheck.PaycheckSeniorDependentsBonus);
        Assert.Equal(2830.77m, paycheck.TotalPaycheckAmount);
    }

    [Fact]
    public async Task GetPaycheckAsync_ShouldAdjustLastPaycheck()
    {
        _employee.Salary = 50000;

        // Regular Paycheck
        var onDate = new DateTime(DateTime.Now.Year, 02, 02);
        var paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(1923.08m, paycheck!.PaycheckAmount);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(461.54m, paycheck.PaycheckBaseBonus);
        Assert.Equal(2384.62m, paycheck.TotalPaycheckAmount);

        // Last paycheck in a year
        onDate = new DateTime(DateTime.Now.Year, 12, 30);
        paycheck = await _service.GetPaycheckAsync(_employee.Id, onDate);

        Assert.NotNull(paycheck);
        Assert.Equal(1923.00m, paycheck!.PaycheckAmount);
        Assert.Equal(12000, paycheck.AnnualBaseBonus);
        Assert.Equal(461.50m, paycheck.PaycheckBaseBonus);
        Assert.Equal(2384.50m, paycheck.TotalPaycheckAmount);
    }
}
