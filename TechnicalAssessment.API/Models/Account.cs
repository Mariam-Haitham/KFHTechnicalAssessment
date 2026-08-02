using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalAssessment.API.Models;

public class Account
{
    public Guid AccountId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CustomerId { get; set; }

    [Required, MaxLength(20)]
    public string AccountNumber { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    public AccountStatus Status { get; set; } = AccountStatus.Active;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties.
    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }
}
