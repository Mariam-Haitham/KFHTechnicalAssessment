using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalAssessment.API.Models;

public class Transaction
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();

    public Guid? FromAccountId { get; set; }

    public Guid? ToAccountId { get; set; }

    [Range(0.01, 10000)]
    public decimal Amount { get; set; }

    public TransactionType TransactionType { get; set; }

    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation properties.
    [ForeignKey(nameof(FromAccountId))]
    public Account? FromAccount { get; set; }

    [ForeignKey(nameof(ToAccountId))]
    public Account? ToAccount { get; set; }
}
