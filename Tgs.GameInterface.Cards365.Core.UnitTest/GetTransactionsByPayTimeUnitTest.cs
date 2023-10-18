using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetTransactionsByPayTimeUnitTest
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
            { "status","-1"},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        var response = new JsonObject
        {
            ["status"] = "1003",
            ["desc"] = "no data"
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByPayTime")
            .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
            .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetTransactionsByPayTimeAsync(
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
            { "status","-1"},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        var response = new JsonObject
        {
            ["status"] = "1",
            ["count"] = 1,
            ["transactions"] = new JsonArray {
                new JsonObject{
                    ["txId"] = "17f24f4aea4611ed853f5254002237ea",
                    ["gameNumber"] = "549-1683182212-585257-1-36",
                    ["userId"] = "4ucmqplxb029",
                    ["extension1"] = "cardp101",
                    ["status"] = "Pay",
                    ["tiles"] = "{x=[d3, d8, b8], z=[d11, d4]}",
                    ["gameId"] = 549,
                    ["chair"] = 0,
                    ["playId"] = 1,
                    ["commission"] = 0.0,
                    ["banker"] = 1,
                    ["profit"] = -9.0,
                    ["validbet"] = 9.0,
                    ["bonus"] = 0.0,
                    ["reward"] = 0.0,
                    ["outMoney"] = 0.0,
                    ["currency"] = "CNY",
                    ["updateTime"] = "05/04/2023 14:37:36.756",
                    ["createTime"] = "05/04/2023 14:37:15.978",
                    ["betTransTime"] = "05/04/2023 14:37:15.905",
                    ["payTransTime"]= "05/04/2023 14:37:36.661",
                    ["betAmount"] = 186.07,
                    ["realBetAmount"] = 9.0,
                    ["payAmount"] = 177.07,
                    ["realPayAmount"] = 0.0
                }
            }
        };

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByPayTime")
            .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
            .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeHttpClient);

        var actual = await client.GetTransactionsByPayTimeAsync(
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
        Assert.Equal("17f24f4aea4611ed853f5254002237ea", actual.Single().TxId);
        Assert.Equal("4ucmqplxb029", actual.Single().UserId);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T14:37:36.756+08:00"), actual.Single().UpdateTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T14:37:15.905+08:00"), actual.Single().BetTransTime);
        Assert.Equal(DateTimeOffset.Parse("2023-05-04T14:37:36.661+08:00"), actual.Single().PayTransTime);
        Assert.Equal(9.0m, actual.Single().Bet);
        Assert.Equal(9.0m, actual.Single().Validbet);
        Assert.Equal(-9.0m, actual.Single().Winloss);
        Assert.Equal(0.0m, actual.Single().Commission);
    }
}
