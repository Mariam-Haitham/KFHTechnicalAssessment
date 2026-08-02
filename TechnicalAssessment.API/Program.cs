using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;
using TechnicalAssessment.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS for Blazor WASM client.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7149", "http://localhost:5086")
              .AllowAnyMethod()
              .AllowAnyHeader()));

// Register the database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=TechnicalAssessment.db"));
    
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed test data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Customers.Any())
    {
        context.Customers.AddRange(
            new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DateOfBirth = new DateOnly(1990, 5, 15), KYCStatus = KYCStatus.Verified },
            new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", DateOfBirth = new DateOnly(1988, 3, 22), KYCStatus = KYCStatus.Verified },
            new Customer { FirstName = "Michael", LastName = "Johnson", Email = "michael.j@example.com", DateOfBirth = new DateOnly(1992, 7, 10), KYCStatus = KYCStatus.Pending },
            new Customer { FirstName = "Sarah", LastName = "Williams", Email = "sarah.w@example.com", DateOfBirth = new DateOnly(1985, 11, 8), KYCStatus = KYCStatus.Rejected },
            new Customer { FirstName = "Robert", LastName = "Brown", Email = "robert.b@example.com", DateOfBirth = new DateOnly(1995, 2, 14), KYCStatus = KYCStatus.Pending }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
