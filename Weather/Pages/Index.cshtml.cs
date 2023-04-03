using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;
using Weather.Domain.Entities;
using Weather.Domain.ViewModels;
using Weather.Models;
using Weather.Services.Interfaces;

namespace Weather.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiKeys _apiKeys;

        private readonly ILogger<IndexModel> _logger;
        private readonly IOptions<ApiKeys> _options;
        private readonly IForecastUserService _forecastUserService;

        [BindProperty]
        public SearchForecastByNameViewModel ModelSearch { get; set; } = default;

        [BindProperty]
        public List<CityWeather> ForecastCities { get; set; } = default;

        [BindProperty]
        public string? Error { get; set; } = default;

        public IndexModel(ILogger<IndexModel> logger, IOptions<ApiKeys> options, IForecastUserService forecastUserService)
        {
            _logger = logger;
            _options = options;
            _apiKeys = options.Value;
            _forecastUserService = forecastUserService;
        }

        private readonly int UId = 1;

        public async Task OnGet()
        {
            //var response = await _forecastUserService.GetCities(UId);
        }

        public async Task OnPostSearch()
        {
            var response = await _forecastUserService.SearchByCityName(UId, ModelSearch, _apiKeys.OpenWeather);

            if (response.HttpCode == HttpStatusCode.OK && response.Data != null)
            {
                ForecastCities.Add(response.Data);
                Error = null;
            } else if (response.Message != null)
            {
                Error = response.Message;
            }
        }
    }
}