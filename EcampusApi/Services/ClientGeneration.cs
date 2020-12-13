using EcampusApi.Constans;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EcampusApi.Services
{
    public static class ClientGeneration
    {
        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Referer", Links.BaseLink + Links.LoginLink);
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("Keep-Alive", "true");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
            return client;
        }
    }
}
