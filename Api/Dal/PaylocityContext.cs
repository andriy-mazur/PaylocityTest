using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Dal;

public class PaylocityContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Dependent> Dependents { get; set; }

    private const string CONFIG_DB_NAME = "ApiDatabase";
    private readonly IConfiguration _configuration;

    public PaylocityContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
       // Connect to sqlite database
       options.UseSqlite($"Data Source={_configuration.GetConnectionString(CONFIG_DB_NAME)}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
            .HasMany<Dependent>(e => e.Dependents)
            .WithOne(e => e.Employee)
            .HasForeignKey(e => e.EmployeeId)
            .HasPrincipalKey(e => e.Id);

        // SeedData(modelBuilder);
    }

    // private static void SeedData(ModelBuilder modelBuilder)
    // {
    //     var rnd = new Random();
    //     const int employeeCount = 40;
    //     var dependentIndex = 0;
    //     for (var i = 1; i <= employeeCount; i++)
    //     {
    //         var employee = new EmployeeFaker(i).Generate();
    //
    //         // Generate none or single Spouse Or Domestic Partner
    //         if (rnd.Next(2) == 0)
    //         {
    //             dependentIndex++;
    //             var relationship = rnd.Next(2) == 0 ? Relationship.Spouse : Relationship.DomesticPartner;
    //             var spouse = new DependentFaker(dependentIndex, employee.Id, relationship).Generate();
    //
    //             modelBuilder.Entity<Dependent>().HasData(spouse);
    //
    //             //employee.Dependents.Add(spouse);
    //         }
    //
    //         // Generate 0 .. 3 children
    //         var childrenCount = rnd.Next(0, 4);
    //         for (var childIndex = 0; childIndex < childrenCount; childIndex++)
    //         {
    //             dependentIndex++;
    //             var child = new DependentFaker(dependentIndex, employee.Id, Relationship.Child).Generate();
    //
    //             modelBuilder.Entity<Dependent>().HasData(child);
    //
    //             //employee.Dependents.Add(child);
    //         }
    //
    //         //_dbContext.Employees.Add(employee);
    //         modelBuilder.Entity<Employee>().HasData(employee);
    //     }
    // }
}
