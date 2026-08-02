using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;
using TechnicalAssessment.API.Models;

namespace TechnicalAssessment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<List<Account>>> GetAccountsByCustomer(Guid customerId)
    {
        var accounts = await _context.Accounts
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
        return Ok(accounts);
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<Account>> GetAccount(Guid accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null)
            return NotFound();
        return Ok(account);
    }
}
