using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS for Blazor WASM client.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("https://localhost:7149")
              .AllowAnyMethod()
              .AllowAnyHeader()));

// Register the database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=TechnicalAssessment.db"));
    
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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
