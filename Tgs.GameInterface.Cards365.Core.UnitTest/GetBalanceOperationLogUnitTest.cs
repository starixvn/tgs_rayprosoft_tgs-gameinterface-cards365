using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetBalanceOperationLogUnitTest
{
    [Fact]
    public async Task Data_Does_Not_Exist()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var ts_code = "1683106362";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "ts_code",ts_code}
        };

        var content = "{\"status\":\"1\",\"result\":[]}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalanceOperationLog")
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
            .GetBalanceOperationLogAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                ts_code: ts_code)
            .ConfigureAwait(false);

        Assert.False(actual);
    }

    [Fact]
    public async Task Data_Exists()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var ts_code = "1683106362";

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "ts_code",ts_code}
        };

        var content = "{\"status\":\"1\",\"result\":[{\"tscode\":\"1683106362\",\"id\":\"4ucmqplxb029\",\"balance\":\"1.0\",\"createdate\":\"03/05/2023 17:33:05\",\"exntesion1\":\"cardp101\"}]}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalanceOperationLog")
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client
            .GetBalanceOperationLogAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                ts_code: ts_code)
            .ConfigureAwait(false);

        Assert.True(actual);
    }
}
