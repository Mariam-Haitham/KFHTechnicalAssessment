using System.Net.Http.Json;
using TechnicalAssessment.BlazorUI.Client.Models;

namespace TechnicalAssessment.BlazorUI.Client.Services;

public class AccountApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private const string BaseUrl = "http://localhost:5135/api";

    public async Task<List<AccountDto>> GetAccountsByCustomerAsync(Guid customerId)
    {
        return await _httpClient.GetFromJsonAsync<List<AccountDto>>($"{BaseUrl}/accounts/customer/{customerId}") ?? [];
    }

    public async Task<List<TransactionDto>> GetAccountTransactionsAsync(Guid accountId, int limit = 10)
    {
        return await _httpClient.GetFromJsonAsync<List<TransactionDto>>($"{BaseUrl}/transactions/account/{accountId}?limit={limit}") ?? [];
    }

    public async Task<bool> TransferFundsAsync(Guid? fromAccountId, Guid? toAccountId, decimal amount)
    {
        var request = new { fromAccountId, toAccountId, amount };
        var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/transactions/transfer", request);
        return response.IsSuccessStatusCode;
    }
}
