using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Models;

namespace TechnicalAssessment.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}