using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetBalanceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var user = "4ucmqplxb029";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "alluser","0"},
            { "users",user}
        };

        var content = "{\"status\":\"1\",\"results\":[],\"count\":\"0\",\"querytime\":\"03/05/2023 17:10:52\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalance")
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
            .GetBalanceAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                user: user)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
    }

    [Fact]
    public async Task This_Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var user = "4ucmqplxb029";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "alluser","0"},
            { "users",user}
        };

        var content = "{\"status\":\"1\",\"results\":[{\"user\":\"4ucmqplxb029\",\"extension1\":\"cardp101\",\"currency\":\"3\",\"balance\":\"0.0\"}],\"count\":\"1\",\"querytime\":\"03/05/2023 17:25:38\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalance")
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
            .GetBalanceAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                user: user)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
    }
}
