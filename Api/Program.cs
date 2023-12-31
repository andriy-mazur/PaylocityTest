using Api.Configurations;
using Api.Dal;
using Api.MappingProfiles;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDependentRepository, DependentRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDependentService, DependentService>();
builder.Services.AddScoped<IPaycheckService, PaycheckService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<PaycheckConfiguration>(builder.Configuration.GetSection("PaycheckConfiguration"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Version = "v1",
                Title = "Employee Benefit Cost Calculation Api",
                Description = "Api to support employee benefit cost calculations"
            }
        );
    }
);
builder.Services.AddDbContextFactory<PaylocityContext>();

var allowLocalhost = "allow localhost";
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            allowLocalhost,
            policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); }
        );
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var context = new PaylocityContext(builder.Configuration))
{
    context.Database.Migrate();
}

app.Run();
