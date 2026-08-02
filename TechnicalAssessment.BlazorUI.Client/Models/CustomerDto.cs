namespace TechnicalAssessment.BlazorUI.Client.Models;

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string KYCStatus { get; set; } = string.Empty;
}
