using Microsoft.Extensions.Options;
using Multilanguage.Application.Abstract;
using Multilanguage.Infrastructure.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Multilanguage.Infrastructure.Localizer
{
    public class StringLocalizerCognitiveService : IStringLocalizerCogniteveService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<CognitiveServiceOptions> _options;

        public StringLocalizerCognitiveService(IHttpClientFactory httpClientFactory,
                                               IOptions<CognitiveServiceOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task<string> Get(string key, string toLanguage)
        {
            string uri = string.Format(_options.Value.EndPoint + "&from={0}&to={1}", "en", toLanguage);
            object[] body = new object[] { new { Text = key } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var httpClient = _httpClientFactory.CreateClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _options.Value.ApiKey);
                request.Headers.Add("Ocp-Apim-Subscription-Region", _options.Value.Location);
                request.Headers.Add("X-ClientTraceId", Guid.NewGuid().ToString());

                var response = await httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<Dictionary<string, List<Dictionary<string, string>>>>>(responseBody);
                var translation = result[0]["translations"][0]["text"];
                return translation;
            }
        }
    }
}
