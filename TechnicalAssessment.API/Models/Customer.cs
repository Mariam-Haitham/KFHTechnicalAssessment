using System.ComponentModel.DataAnnotations;

namespace TechnicalAssessment.API.Models;

public class Customer
{
    // globally unique identifier.
    public Guid CustomerId { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public required string FirstName { get; set; }

    [Required, MaxLength(100)]
    public required string LastName { get; set; }

    [Required, EmailAddress, MaxLength(256)]
    public required string Email { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public KYCStatus KYCStatus { get; set; } = KYCStatus.Pending;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
}
