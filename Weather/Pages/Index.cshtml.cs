using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Weather.Models;

namespace Weather.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiKeys _apiKeys;

        private readonly ILogger<IndexModel> _logger;
        private readonly IOptions<ApiKeys> _options;

        public IndexModel(ILogger<IndexModel> logger, IOptions<ApiKeys> options)
        {
            _logger = logger;
            _options = options;
            _apiKeys = options.Value;
        }

        public void OnGet()
        {
        }
    }
}