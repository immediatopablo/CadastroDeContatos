using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ViaCepResponse> GetAddressByCepAsync(string cep)
    {
        var url = $"https://viacep.com.br/ws/{cep}/json/";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null; // Ou trate o erro como preferir
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ViaCepResponse>(content);
    }
}

public class ViaCepResponse
{
    public required string Cep { get; set; }
    public string? Logradouro { get; set; }
    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Localidade { get; set; }
    public string? Uf { get; set; }
    public string? Ibge { get; set; }
    public string? Gia { get; set; }
    public string? Ddd { get; set; }
    public string? Siafi { get; set; }
}
