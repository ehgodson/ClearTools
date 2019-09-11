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
        string LastResponseString { get; }
        Task<TEntity> GetAsync<TEntity>(string requestUrl);
        Task<TEntity> GetAsync<TEntity>(string requestUrl, string token);
        Task<TEntity> GetAsync<TEntity>(string requestUrl, string token, Dictionary<string, string> headers);

        Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, bool ensureSuccess = true);
        Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, string token, bool ensureSuccess = true);
        Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, bool ensureSuccess = true);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, bool ensureSuccess = true);
        Task<TResult> PostAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true);

        Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, bool ensureSuccess = true);
        Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, string token, bool ensureSuccess = true);
        Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true);
        Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, bool ensureSuccess = true);
        Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, bool ensureSuccess = true);
        Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true);

        TEntity Serialize<TEntity>(string data);
    }
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private string _lastResponseString;

        public ApiClient() => _httpClient = new HttpClient();

        #region get data

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
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(_lastResponseString);
        }

        #endregion

        #region post data

        public async Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, bool ensureSuccess = true) =>
            await PostAsync(requestUrl, content, string.Empty, ensureSuccess);

        public async Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, string token, bool ensureSuccess = true) =>
            await PostAsync(requestUrl, content, token, new Dictionary<string, string>(), ensureSuccess);

        public async Task<string> PostAsync<TEntity>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.PostAsync(requestUrl, CreateHttpContent(content));
            if (ensureSuccess) response.EnsureSuccessStatusCode();
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return _lastResponseString;
        }

        // ========

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
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(_lastResponseString);
        }

        #endregion

        #region put data

        public async Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, bool ensureSuccess = true) =>
            await PutAsync(requestUrl, content, string.Empty, ensureSuccess);

        public async Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, string token, bool ensureSuccess = true) =>
            await PutAsync(requestUrl, content, token, new Dictionary<string, string>(), ensureSuccess);

        public async Task<string> PutAsync<TEntity>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.PutAsync(requestUrl, CreateHttpContent(content));
            if (ensureSuccess) response.EnsureSuccessStatusCode();
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return _lastResponseString;
        }

        // ========

        public async Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, bool ensureSuccess = true) =>
            await PutAsync<TEntity, TResult>(requestUrl, content, string.Empty, ensureSuccess);

        public async Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, bool ensureSuccess = true) =>
            await PutAsync<TEntity, TResult>(requestUrl, content, token, new Dictionary<string, string>(), ensureSuccess);

        public async Task<TResult> PutAsync<TEntity, TResult>(string requestUrl, TEntity content, string token, Dictionary<string, string> headers, bool ensureSuccess = true)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.PutAsync(requestUrl, CreateHttpContent(content));
            if (ensureSuccess) response.EnsureSuccessStatusCode();
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(_lastResponseString);
        }

        #endregion

        #region delete data

        public async Task<string> DeleteAsync(string requestUrl, bool ensureSuccess = true) =>
            await DeleteAsync(requestUrl, string.Empty, ensureSuccess);

        public async Task<string> DeleteAsync(string requestUrl, string token, bool ensureSuccess = true) =>
            await DeleteAsync(requestUrl, token, new Dictionary<string, string>(), ensureSuccess);

        public async Task<string> DeleteAsync(string requestUrl, string token, Dictionary<string, string> headers, bool ensureSuccess = true)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.DeleteAsync(requestUrl);
            if (ensureSuccess) response.EnsureSuccessStatusCode();
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return _lastResponseString;
        }

        // =======

        public async Task<TEntity> DeleteAsync<TEntity>(string requestUrl) =>
            await DeleteAsync<TEntity>(requestUrl, string.Empty, new Dictionary<string, string>());

        public async Task<TEntity> DeleteAsync<TEntity>(string requestUrl, string token) =>
            await DeleteAsync<TEntity>(requestUrl, token, new Dictionary<string, string>());

        public async Task<TEntity> DeleteAsync<TEntity>(string requestUrl, string token, Dictionary<string, string> headers)
        {
            AddToken(token);
            AddHeaders(headers);
            var response = await _httpClient.DeleteAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            _lastResponseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TEntity>(_lastResponseString);
        }

        #endregion

        #region misc funtions

        public TEntity Serialize<TEntity>(string data) => JsonConvert.DeserializeObject<TEntity>(data);

        private HttpContent CreateHttpContent<T>(T content) =>
            new StringContent(JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings), Encoding.UTF8, "application/json");

        private static JsonSerializerSettings MicrosoftDateFormatSettings => new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };

        public string LastResponseString => _lastResponseString;

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

        #endregion
    }
}