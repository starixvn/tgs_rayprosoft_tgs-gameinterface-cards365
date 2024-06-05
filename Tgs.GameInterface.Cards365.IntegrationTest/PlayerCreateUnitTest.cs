using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.IntegrationTest;
public class PlayerCreateUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var playerCode = "4ucmqplxb029";
		var currency = 3;
		var fakeService = Substitute.For<ICommandService<PlayerCreateCommand>>();
		var web = new GameInterfaceCards365Application(builder => _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeService)));

		var sut = new Client.GameInterfaceCards365Client(new Protos.GameInterfaceCards365.GameInterfaceCards365Client(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		await sut.PlayerCreateAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			playerCode: playerCode,
			currency: currency);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is(new PlayerCreateCommand(
				ApiUrl: apiUrl,
				Website: website,
				Cert: cert,
				Extension1: extension1,
				PlayerCode: playerCode,
				Currency: currency)),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
