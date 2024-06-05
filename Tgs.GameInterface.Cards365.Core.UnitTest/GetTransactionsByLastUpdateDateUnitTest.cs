using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetTransactionsByLastUpdateDateUnitTest
{
	[Fact]
	public async Task No_Data()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var lastupdatedate = (long)0;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "extension1",extension1},
			{ "status","-1"},
			{ "lastupdatedate",lastupdatedate.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1003",
			["desc"] = "no data"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByLastUpdateDate")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetTransactionsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			lastupdatedate: lastupdatedate)
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();

		Assert.NotNull(actual);
		Assert.False(actual.Any());
	}

	[Fact]
	public async Task Normal_Process_Status_Is_Bet()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var lastupdatedate = (long)0;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "extension1",extension1},
			{ "status","-1"},
			{ "lastupdatedate",lastupdatedate.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["count"] = 1,
			["transactions"] = new JsonArray {
				new JsonObject{
					["txId"] ="6e91c688ea4011ed853f5254002237ea",
					["gameNumber"] = "549-1683179782-585257-1-36",
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
					["updateTime"] ="05/04/2023 13:56:44.962",
					["createTime"] ="05/04/2023 13:56:44.962",
					["betTransTime"] = "05/04/2023 13:56:44.886",
					["payTransTime"] ="",
					["betAmount"] =175.73,
					["realBetAmount"] = 0.0,
					["payAmount"] = 0.0,
					["realPayAmount"] = 0.0
				}
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByLastUpdateDate")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetTransactionsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			lastupdatedate: lastupdatedate)
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal("6e91c688ea4011ed853f5254002237ea", actual.Single().TxId);
		Assert.Equal("4ucmqplxb029", actual.Single().UserId);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:56:44.962+08:00"), actual.Single().UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:56:44.886+08:00"), actual.Single().BetTransTime);
		Assert.Equal(DateTimeOffset.UnixEpoch, actual.Single().PayTransTime);
		Assert.Equal(0.0m, actual.Single().Bet);
		Assert.Equal(0.0m, actual.Single().Validbet);
		Assert.Equal(0.0m, actual.Single().Winloss);
		Assert.Equal(0.0m, actual.Single().Commission);
	}

	[Fact]
	public async Task Normal_Process_Status_Is_Pay_Bet_On_Both_Sides()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var lastupdatedate = (long)0;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "extension1",extension1},
			{ "status","-1"},
			{ "lastupdatedate",lastupdatedate.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["count"] = 1,
			["transactions"] = new JsonArray {
				new JsonObject{
					["txId"] ="cf18a594ea4011ed853f5254002237ea",
					["gameNumber"] = "549-1683179956-585257-1-39",
					["userId"] = "4ucmqplxb029",
					["extension1"] = "cardp101",
					["status"] = "Pay",
					["tiles"] = "{x=[d5, c13, d10], z=[b4, c7, a10]}",
					["gameId"] = 549,
					["chair"] = 0,
					["playId"] = 1,
					["commission"] = 0.15,
					["banker"] = 1,
					["profit"] = -0.15,
					["validbet"] = 0.15,
					["bonus"] = 0.0,
					["reward"] = 0.0,
					["outMoney"] = 0.0,
					["currency"] = "CNY",
					["updateTime"] = "05/04/2023 13:59:46.445",
					["createTime"] = "05/04/2023 13:59:24.362",
					["betTransTime"] = "05/04/2023 13:59:24.288",
					["payTransTime"] = "05/04/2023 13:59:46.327",
					["betAmount"] = 189.31,
					["realBetAmount"] = 0.0,
					["payAmount"] = 189.16,
					["realPayAmount"] = 0.0
				}
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByLastUpdateDate")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetTransactionsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			lastupdatedate: lastupdatedate)
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal("cf18a594ea4011ed853f5254002237ea", actual.Single().TxId);
		Assert.Equal("4ucmqplxb029", actual.Single().UserId);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:59:46.445+08:00"), actual.Single().UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:59:24.288+08:00"), actual.Single().BetTransTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:59:46.327+08:00"), actual.Single().PayTransTime);
		Assert.Equal(0.0m, actual.Single().Bet);
		Assert.Equal(0.15m, actual.Single().Validbet);
		Assert.Equal(-0.15m, actual.Single().Winloss);
		Assert.Equal(0.15m, actual.Single().Commission);
	}

	[Fact]
	public async Task Normal_Process_Status_Is_Pay_Loss()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var lastupdatedate = (long)0;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "extension1",extension1},
			{ "status","-1"},
			{ "lastupdatedate",lastupdatedate.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["count"] = 1,
			["transactions"] = new JsonArray {
				new JsonObject{
					["txId"] ="87032f02ea3d11ed853f5254002237ea",
					["gameNumber"] = "549-1683178547-585257-1-60",
					["userId"] = "4ucmqplxb029",
					["extension1"] = "cardp101",
					["status"] = "Pay",
					["tiles"] = "{x=[c11, d10, b12], z=[c1, b5]}",
					["gameId"] = 549,
					["chair"] = 0,
					["playId"] = 1,
					["commission"] = 0.0,
					["banker"] = 1,
					["profit"] = -33.0,
					["validbet"] = 33.0,
					["bonus"] = 0.0,
					["reward"] = 0.0,
					["outMoney"] = 0.0,
					["currency"] = "CNY",
					["updateTime"] = "05/04/2023 13:36:16.185",
					["createTime"] = "05/04/2023 13:35:54.963",
					["betTransTime"] = "05/04/2023 13:35:54.886",
					["payTransTime"] = "05/04/2023 13:36:16.092",
					["betAmount"] = 108.73,
					["realBetAmount"] = 33.0,
					["payAmount"] = 75.73,
					["realPayAmount"] = 0.0
				}
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByLastUpdateDate")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetTransactionsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			lastupdatedate: lastupdatedate)
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal("87032f02ea3d11ed853f5254002237ea", actual.Single().TxId);
		Assert.Equal("4ucmqplxb029", actual.Single().UserId);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.185+08:00"), actual.Single().UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:35:54.886+08:00"), actual.Single().BetTransTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T13:36:16.092+08:00"), actual.Single().PayTransTime);
		Assert.Equal(33m, actual.Single().Bet);
		Assert.Equal(33m, actual.Single().Validbet);
		Assert.Equal(-33.0m, actual.Single().Winloss);
		Assert.Equal(0.0m, actual.Single().Commission);
	}

	[Fact]
	public async Task Normal_Process_Status_Is_Pay_Win()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var lastupdatedate = (long)0;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "extension1",extension1},
			{ "status","-1"},
			{ "lastupdatedate",lastupdatedate.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["count"] = 1,
			["transactions"] = new JsonArray {
				new JsonObject{
					["txId"] = "0cede24cea1b11ed853f5254002237ea",
					["gameNumber"] = "549-1683163727-585257-1-11",
					["userId"] = "4ucmqplxb029",
					["extension1"] = "cardp101",
					["status"] = "Pay",
					["tiles"] = "{x=[c11, d5, c11], z=[c7, d4, b12]}",
					["gameId"] = 549,
					["chair"] = 0,
					["playId"] = 1,
					["commission"] = 0.15,
					["banker"] = 1,
					["profit"] = 4.85,
					["validbet"] = 4.85,
					["bonus"] = 0.0,
					["reward"] = 0.0,
					["outMoney"] = 0.0,
					["currency"] = "CNY",
					["updateTime"] = "05/04/2023 09:29:31.476",
					["createTime"] = "05/04/2023 09:29:09.777",
					["betTransTime"] = "05/04/2023 09:29:09.699",
					["payTransTime"] = "05/04/2023 09:29:31.358",
					["betAmount"] = 100.0,
					["realBetAmount"] = 5.0,
					["payAmount"] = 104.85,
					["realPayAmount"] = 5.0
				}
			}
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getTransactionsByLastUpdateDate")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetTransactionsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			lastupdatedate: lastupdatedate)
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal("0cede24cea1b11ed853f5254002237ea", actual.Single().TxId);
		Assert.Equal("4ucmqplxb029", actual.Single().UserId);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:31.476+08:00"), actual.Single().UpdateTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:09.699+08:00"), actual.Single().BetTransTime);
		Assert.Equal(DateTimeOffset.Parse("2023-05-04T09:29:31.358+08:00"), actual.Single().PayTransTime);
		Assert.Equal(5m, actual.Single().Bet);
		Assert.Equal(4.85m, actual.Single().Validbet);
		Assert.Equal(4.85m, actual.Single().Winloss);
		Assert.Equal(0.15m, actual.Single().Commission);
	}
}
