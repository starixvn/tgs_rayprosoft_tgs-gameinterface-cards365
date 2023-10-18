using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Exceptions;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class LogoutUnitTest
{
    [Fact]
    public async Task Exception()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var user = "4ucmqplxb029";
        var extension1 = "cardp101";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "extension1",extension1}
        };

        var response = new JsonObject
        {
            ["status"] = "0"
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/logout")
            .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
            .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        _ = await Assert.ThrowsAsync<GameInterfaceException>(() => client
            .LogoutAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                extension1: extension1,
                user: user)
            .AsTask());
    }

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var user = "4ucmqplxb029";
        var extension1 = "cardp101";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "extension1",extension1}
        };

        var response = new JsonObject
        {
            ["status"] = "1"
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/logout")
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
                .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), mediaType: MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json)));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        await client
           .LogoutAsync(
               apiUrl: apiUrl,
               website: website,
               cert: cert,
               extension1: extension1,
               user: user);
    }
}
