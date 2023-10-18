using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetTransactionsByBetTimeUnitTest
{
    [Fact]
    public async Task No_Data()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var extension1 = "cardp101";
        var startTime = DateTimeOffset.Now.AddMinutes(-10);
        var endTime = DateTimeOffset.Now;

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "extension1",extension1},
            { "status",0.ToString()},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        var response = new JsonObject
        {
            ["status"] = "1003",
            ["desc"] = "no data"
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByBetTime")
            .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
            .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetTransactionsByBetTimeAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            extension1: extension1,
            startTime: startTime,
            endTime: endTime)
            .ToArrayAsync()
            ?? Enumerable.Empty<TransactionInfo>();

        Assert.NotNull(actual);
        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
        var website = "card365pega001";
        var cert = "40D237D0A995DFEAE0590F99552D3F6F";
        var extension1 = "cardp101";
        var startTime = DateTimeOffset.Now.AddMinutes(-10);
        var endTime = DateTimeOffset.Now;

        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "extension1",extension1},
            { "status",0.ToString()},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        var response = new JsonObject
        {
            ["status"] = "1",
            ["count"] = 1,
            ["transactions"] = new JsonArray {
                new JsonObject{
                    ["txId"] = "a1c6ad1cea4411ed853f5254002237ea",
                    ["gameNumber"] = "549-1683181598-585257-1-17",
                    ["userId"] = "4ucmqplxb029",
                    ["extension1"] = "cardp101",
                    ["status"] = "Bet",
                    ["tiles"] = null,
                    ["gameId"] = 549,
                    ["chair"] = 0,
                    ["playId"] = 1,
                    ["commission"] = 0.0,
                    ["banker"] = 0,
                    ["profit"] = 0.0,
                    ["validbet"] = 0.0,
                    ["bonus"] = 0.0,
                    ["reward"] = 0.0,
                    ["outMoney"] = 0.0,
                    ["currency"] = "CNY",
                    ["updateTime"] = "05/04/2023 14:26:46.364",
                    ["createTime"] = "05 /04/2023 14:26:46.364",
                    ["betTransTime"] = "05 /04/2023 14:26:46.289",
                    ["payTransTime"] = "",
                    ["betAmount"] = 181.07,
                    ["realBetAmount"] = 0.0,
                    ["payAmount"] = 0.0,
                    ["realPayAmount"] = 0.0
                }
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByBetTime")
            .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
            .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetTransactionsByBetTimeAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            extension1: extension1,
            startTime: startTime,
            endTime: endTime)
            .ToArrayAsync()
            ?? Enumerable.Empty<TransactionInfo>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal("a1c6ad1cea4411ed853f5254002237ea", actual.Single().TxId);
        Assert.Equal("4ucmqplxb029", actual.Single().UserId);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T14:26:46.364+08:00"), actual.Single().UpdateTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T14:26:46.289+08:00"), actual.Single().BetTransTime);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.Single().PayTransTime);
        Assert.Equal(0.0m, actual.Single().Bet);
        Assert.Equal(0.0m, actual.Single().Validbet);
        Assert.Equal(0.0m, actual.Single().Winloss);
        Assert.Equal(0.0m, actual.Single().Commission);
    }
}
