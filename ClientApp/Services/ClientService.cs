using System.Net;
using System.Text.Json;
using ClientApp.Repositories;
using LabsDB.Entity;

namespace ClientApp.Services;

public class ClientService : IClientRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<House>> GetAllHouses()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestVersion = HttpVersion.Version20;
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
        try
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5173/client/get")
            {
                Version = new Version(2, 0),
            };
            var responseMessage = await client.SendAsync(req);
            if (!responseMessage.IsSuccessStatusCode)
                return Enumerable.Empty<House>();

            var str = await responseMessage.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<House>>(str);
            return res ?? Enumerable.Empty<House>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Enumerable.Empty<House>();
        }
    }
}