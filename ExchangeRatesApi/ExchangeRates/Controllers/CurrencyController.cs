using Microsoft.AspNetCore.Mvc;
namespace ExchangeRatesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "b0cba1242cbaeedf39a56a3f";
        private const string BaseUrl = "https://v6.exchangerate-api.com/v6";

        public CurrencyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{ApiKey}/latest/USD");
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();

                if (data?.conversion_rates != null)
                {
                    var currencies = data.conversion_rates.Keys.ToList();
                    return Ok(currencies);
                }

                return StatusCode(500, "Failed to fetch currencies");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("exchange-rates/{baseCurrency}")]
        public async Task<IActionResult> GetExchangeRates(string baseCurrency)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/{ApiKey}/latest/{baseCurrency}");
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadFromJsonAsync<ExchangeRateResponse>();

                if (data?.conversion_rates != null)
                {
                    return Ok(data.conversion_rates);
                }

                return StatusCode(500, "Failed to fetch exchange rates");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

    public class ExchangeRateResponse
    {
        public string base_code { get; set; }
        public Dictionary<string, decimal> conversion_rates { get; set; }
    }

}

