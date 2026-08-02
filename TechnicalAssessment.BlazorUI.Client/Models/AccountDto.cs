using System.Text.Json.Serialization;

namespace TechnicalAssessment.BlazorUI.Client.Models;

public class AccountDto
{
    [JsonPropertyName("accountId")]
    public Guid AccountId { get; set; }

    [JsonPropertyName("customerId")]
    public Guid CustomerId { get; set; }

    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; set; } = string.Empty;

    [JsonPropertyName("accountType")]
    public int AccountType { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
}

public class TransactionDto
{
    [JsonPropertyName("transactionId")]
    public Guid TransactionId { get; set; }

    [JsonPropertyName("fromAccountId")]
    public Guid? FromAccountId { get; set; }

    [JsonPropertyName("toAccountId")]
    public Guid? ToAccountId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("transactionType")]
    public int TransactionType { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}
