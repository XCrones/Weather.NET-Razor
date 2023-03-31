using System.Text.Json;
using Weather.Services.Interfaces;

namespace Weather.Services.Implements
{
    public class HttpClientService : IHttpClientService
    {
        private readonly JsonSerializerOptions _options;

        public HttpClientService()
        {
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TD?> Get<TD>(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(url).Result;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TD>(content, _options);
        }
    }
}
