using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Clear
{
    public interface IApiClient
    {
        Task<TEntity> GetAsync<TEntity>(string requestUrl);
        Task<TEntity> GetAsync<TEntity>(string requestUrl, string token);
        Task<TEntity> GetAsync<TEntity>(string requestUrl, string token, Dictionary<string, string> headers);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, bool ensureSuccess = true);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, bool ensureSuccess = true);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true);
        TEntity Serialize<TEntity>(string data);
    }
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient() => _httpClient = new HttpClient();

        public async Task<TEntity> GetAsync<TEntity>(string requestUrl) =>
            await GetAsync<TEntity>(requestUrl, string.Empty, new Dictionary<string, string>());

        public async Task<TEntity> GetAsync<TEntity>(string requestUrl, string token) =>
            await GetAsync<TEntity>(requestUrl, token, new Dictionary<string, string>());

        public async Task<TEntity> GetAsync<TEntity>(string requestUrl, string token, Dictionary<string, string> headers)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(data);
        }

        public async Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, bool ensureSuccess = true) =>
            await PostAsync<TEntity, TResult>(requestUrl, content, string.Empty, ensureSuccess);

        public async Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, bool ensureSuccess = true) =>
            await PostAsync<TEntity, TResult>(requestUrl, content, token, new Dictionary<string, string>(), ensureSuccess);

        public async Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.PostAsync(requestUrl, CreateHttpContent(content));
            if (ensureSuccess) response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(data);
        }

        public TEntity Serialize<TEntity>(string data) => JsonConvert.DeserializeObject<TEntity>(data);

        private HttpContent CreateHttpContent<T>(T content) =>
            new StringContent(JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings), Encoding.UTF8, "application/json");

        private static JsonSerializerSettings MicrosoftDateFormatSettings => new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };

        private void AddToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return;
            AddHeaders(new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            });
        }

        private void AddHeaders(Dictionary<string, string> headers)
        {
            if (headers == null) return;
            foreach (var header in headers)
            {
                _httpClient.DefaultRequestHeaders.Remove(header.Key);
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}