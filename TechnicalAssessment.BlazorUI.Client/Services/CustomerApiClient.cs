using System.Net.Http.Json;
using TechnicalAssessment.BlazorUI.Client.Models;

namespace TechnicalAssessment.BlazorUI.Client.Services;

public class CustomerApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    private const string BaseUrl = "http://localhost:5135/api/customers";

    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>(BaseUrl) ?? [];
    }

    public async Task<List<CustomerDto>> SearchCustomersAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return await GetAllCustomersAsync();

        return await _httpClient.GetFromJsonAsync<List<CustomerDto>>($"{BaseUrl}/search?name={Uri.EscapeDataString(name)}") ?? [];
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid customerId)
    {
        return await _httpClient.GetFromJsonAsync<CustomerDto>($"{BaseUrl}/{customerId}");
    }
}
