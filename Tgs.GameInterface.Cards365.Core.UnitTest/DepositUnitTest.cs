using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Exceptions;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class DepositUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var user = "4ucmqplxb029";
		var amount = 1m;
		var ts_code = "D1714538127095910400";
		var extension1 = "cardp101";

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "user",user},
			{ "balance",amount.ToString()},
			{ "ts_code",ts_code},
			{ "extension1",extension1}
		};

		var response = new JsonObject
		{
			["status"] = "1"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/deposit")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.DepositAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			user: user,
			ts_code: ts_code,
			amount: amount);

		Assert.Equal(response.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task This_Player_Does_Not_Exist()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var user = "4ucmqplxb029";
		var amount = 1m;
		var ts_code = "D1714538127095910400";
		var extension1 = "cardp101";

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "user",user},
			{ "balance",amount.ToString()},
			{ "ts_code",ts_code},
			{ "extension1",extension1}
		};

		var response = new JsonObject
		{
			["status"] = "1032"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/deposit")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<GameInterfaceException>(
			() => client.DepositAsync(
				apiUrl: apiUrl,
				website: website,
				cert: cert,
				extension1: extension1,
				user: user,
				ts_code: ts_code,
				amount: amount)
			.AsTask());
	}
}
