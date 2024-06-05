using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.IntegrationTest;

public class PlayerBalanceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var playerCode = "4ucmqplxb029";

		var web = new GameInterfaceCards365Application(builder => _ = builder.ConfigureServices(services =>
		{
			var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();

			var query = new PlayerBalanceQuery(
			ApiUrl: apiUrl,
			Website: website,
			Cert: cert,
			PlayerCode: playerCode);

			_ = fakeService.GetAsync(
				query: Arg.Is(query),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new PlayerBalanceResult(
				IsPlayerExist: true,
				Balance: 100m));

			_ = services.AddTransient(_ => fakeService);
		}));

		var sut = new Client.GameInterfaceCards365Client(new Protos.GameInterfaceCards365.GameInterfaceCards365Client(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		var actual = await sut.PlayerBalanceAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			playerCode: playerCode);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(100m, actual.Balance.ToDecimal().GetValueOrDefault(0.0m));
	}
}
