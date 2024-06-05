using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetBalancesUnitTest
{
	[Fact]
	public async Task Player_Balance_Is_Greater_Than_Zero()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "alluser","1"}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["results"] = new JsonArray
			{
				new JsonObject
				{
					["id"]= "4ucmqplxb029",
					["currency"]= "3",
					["exntesion1"]= "cardp101",
					["balance"]= "100.0"
				}
			},
			["count"] = "1",
			["querytime"] = "03/05/2023 17:28:20"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalance")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetBalancesAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert)
			.ToArrayAsync()
			?? Enumerable.Empty<BalanceInfo>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
	}

	[Fact]
	public async Task Player_Balance_Is_Zero()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "alluser","1"}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["results"] = new JsonArray
			{
				new JsonObject
				{
					["id"]= "4ucmqplxb029",
					["currency"]= "3",
					["exntesion1"]= "cardp101",
					["balance"]= "0.0"
				}
			},
			["count"] = "1",
			["querytime"] = "03/05/2023 17:28:20"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getBalance")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetBalancesAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert)
			.ToArrayAsync()
			?? Enumerable.Empty<BalanceInfo>();

		Assert.NotNull(actual);
		Assert.False(actual.Any());
	}
}
