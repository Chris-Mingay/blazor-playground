using System.Net.Http.Json;
using Blazor.Shared.Commands.Auth.Login;
using Blazor.Shared.Commands.Auth.Refresh;
using Blazor.Shared.Commands.Auth.Register;

namespace Blazor.App;

public class DataService : IDataService
{
    private readonly HttpClient _httpClient;

    public DataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<string>> Super()
    {
        var response = await _httpClient.GetFromJsonAsync<List<string>>("super");
        return response;
    }
    
    public async Task<List<string>> Duper()
    {
        var response = await _httpClient.GetFromJsonAsync<List<string>>("duper");
        return response;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}