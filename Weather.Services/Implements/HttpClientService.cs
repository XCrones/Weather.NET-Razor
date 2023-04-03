﻿using System.Text.Json;
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
            HttpClient httpClient = new() 
            {
                Timeout = TimeSpan.FromSeconds(5),
            };
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TD>(content, _options);
        }
    }
}
