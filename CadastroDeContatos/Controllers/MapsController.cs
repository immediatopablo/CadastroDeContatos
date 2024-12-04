using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[Route("Maps")]
public class MapsController : Controller
{
    // URL base da API do Google Maps
    private const string GoogleMapsApiUrl = "https://maps.googleapis.com/maps/api/js";
    // Sua API key do Google Maps
    private const string ApiKey = "AIzaSyDdfmKfdDgHw0L7DyeWvcE1HTOTwOP1z9c";

    // Rota para inicializar o mapa com callback
    [HttpGet("LoadGoogleMapsAPI")]
    public async Task<IActionResult> LoadGoogleMapsAPI([FromQuery] string callback)
    {
        using var httpClient = new HttpClient();
        // Monta a URL com os parâmetros necessários
        var queryParams = $"?key={ApiKey}&callback={callback}";
        var response = await httpClient.GetAsync(GoogleMapsApiUrl + queryParams);

        // Verifica se a resposta foi bem-sucedida
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/javascript");
        }

        // Retorna erro em caso de falha
        return StatusCode((int)response.StatusCode);
    }

    // Rota para carregar bibliotecas específicas (Ex: places)
    [HttpGet("LoadLibrary")]
    public async Task<IActionResult> LoadLibrary([FromQuery] string libraries)
    {
        using var httpClient = new HttpClient();
        // Monta a URL com os parâmetros necessários
        var queryParams = $"?key={ApiKey}&libraries={libraries}";
        var response = await httpClient.GetAsync(GoogleMapsApiUrl + queryParams);

        // Verifica se a resposta foi bem-sucedida
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/javascript");
        }

        // Retorna erro em caso de falha
        return StatusCode((int)response.StatusCode);
    }
}
