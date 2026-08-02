using System.Text.Json.Serialization;

namespace TechnicalAssessment.BlazorUI.Client.Models;

public class CustomerDto
{
    [JsonPropertyName("customerId")]
    public Guid CustomerId { get; set; }

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("dateOfBirth")]
    public DateOnly DateOfBirth { get; set; }

    [JsonPropertyName("kycStatus")]
    public int KYCStatus { get; set; }
}
