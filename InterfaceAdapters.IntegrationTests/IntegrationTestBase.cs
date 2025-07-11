using System.Text;
using Newtonsoft.Json;

namespace InterfaceAdapters.IntegrationTests
{
    public class IntegrationTestBase
    {
        protected readonly HttpClient Client;

        protected IntegrationTestBase(HttpClient client)
        {
            Client = client;
        }

        protected async Task<T> PostAndDeserializeAsync<T>(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(body)!;
        }

        protected async Task<HttpResponseMessage> PostAsync(string url, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await Client.PostAsync(url, content);
        }

        protected async Task<T> GetAndDeserializeAsync<T>(string url)
        {
            var response = await Client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(body)!;
        }

        protected async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await Client.GetAsync(url);
        }


    }
}
