using System.Net;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Cards365.Core.Exceptions;

namespace Tgs.GameInterface.Cards365.Core.UnitTest;

public class GetKeyUnitTest
{
	[Fact]
	public async Task Exception()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var user = "4ucmqplxb029";
		var userName = "4ucmqplxb029";
		var extension1 = "cardp101";
		var currency = 3;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "user",user},
			{ "userName",userName},
			{ "extension1",extension1},
			{ "currency",currency.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1001"
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getKey")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<GameInterfaceException>(
			() => client.GetKeyAsync(
				apiUrl: apiUrl,
				website: website,
				cert: cert,
				extension1: extension1,
				user: user,
				userName: userName,
				currency: currency)
			.AsTask());
	}

	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var user = "4ucmqplxb029";
		var userName = "4ucmqplxb029";
		var extension1 = "cardp101";
		var currency = 3;

		var parameters = new Dictionary<string, string>
		{
			{ "cert",cert},
			{ "user",user},
			{ "userName",userName},
			{ "extension1",extension1},
			{ "currency",currency.ToString()}
		};

		var response = new JsonObject
		{
			["status"] = "1",
			["key"] = "VzFob3BnV3I2Z3NLRHUweis1ekVEM1NvSUd1TGl6dlFicmU3cHo0T3pUYkgzeGlnTDdFYzAwTEtackk5dy9Mbg=="
		};

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, $"/api/{website}/getKey")
			.WithHeaders("Content-Type", "application/x-www-form-urlencoded")
			.WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeHttpClient);

		var actual = await client.GetKeyAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			user: user,
			userName: userName,
			currency: currency);

		Assert.NotNull(actual);
		Assert.Equal("VzFob3BnV3I2Z3NLRHUweis1ekVEM1NvSUd1TGl6dlFicmU3cHo0T3pUYkgzeGlnTDdFYzAwTEtackk5dy9Mbg==", actual);
	}
}
