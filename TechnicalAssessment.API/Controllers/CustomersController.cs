using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;

namespace TechnicalAssessment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomersController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetCustomers(string? search)
    {
        var query = _context.Customers
            .AsNoTracking()
            .AsQueryable();


        if (!string.IsNullOrWhiteSpace(search))
        {
            // Case-insensitive name search.
            var searchLower = search.ToLower();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(searchLower) ||
                c.LastName.ToLower().Contains(searchLower));
        }


        var customers = await query.ToListAsync();


        return Ok(customers);
    }
}