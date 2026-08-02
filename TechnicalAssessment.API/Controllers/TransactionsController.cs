using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalAssessment.API.Data;
using TechnicalAssessment.API.Models;

namespace TechnicalAssessment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<List<Transaction>>> GetAccountTransactions(Guid accountId, [FromQuery] int limit = 10)
    {
        var transactions = await _context.Transactions
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.Timestamp)
            .Take(limit)
            .ToListAsync();
        return Ok(transactions);
    }

    [HttpPost("transfer")]
    public async Task<ActionResult> TransferFunds([FromBody] TransferRequest request)
    {
        if (request.Amount <= 0 || request.Amount > 10000)
            return BadRequest("Invalid transfer amount.");

        Account? fromAccount = null;
        Account? toAccount = null;

        // Get from account if provided (Withdrawal or Transfer)
        if (request.FromAccountId.HasValue && request.FromAccountId != Guid.Empty)
        {
            fromAccount = await _context.Accounts.FindAsync(request.FromAccountId);
            if (fromAccount == null)
                return NotFound("From account not found.");
            if (fromAccount.Status != AccountStatus.Active)
                return BadRequest("From account is not active.");
            if (fromAccount.Balance < request.Amount)
                return BadRequest("Insufficient balance.");
        }

        // Get to account if provided (Deposit or Transfer)
        if (request.ToAccountId.HasValue && request.ToAccountId != Guid.Empty)
        {
            toAccount = await _context.Accounts.FindAsync(request.ToAccountId);
            if (toAccount == null)
                return NotFound("To account not found.");
            if (toAccount.Status != AccountStatus.Active)
                return BadRequest("To account is not active.");
        }

        // Determine transaction type
        TransactionType transactionType = TransactionType.Transfer;
        if (fromAccount == null && toAccount != null)
            transactionType = TransactionType.Deposit;
        else if (fromAccount != null && toAccount == null)
            transactionType = TransactionType.Withdrawal;

        // Create transaction
        var transaction = new Transaction
        {
            FromAccountId = fromAccount?.AccountId,
            ToAccountId = toAccount?.AccountId,
            Amount = request.Amount,
            TransactionType = transactionType,
            Status = TransactionStatus.Completed
        };

        // Update balances
        if (fromAccount != null)
            fromAccount.Balance -= request.Amount;
        if (toAccount != null)
            toAccount.Balance += request.Amount;

        _context.Transactions.Add(transaction);
        if (fromAccount != null)
            _context.Accounts.Update(fromAccount);
        if (toAccount != null)
            _context.Accounts.Update(toAccount);

        await _context.SaveChangesAsync();
        return Ok();
    }
}

public class TransferRequest
{
    public Guid? FromAccountId { get; set; }
    public Guid? ToAccountId { get; set; }
    public decimal Amount { get; set; }
}
