using System.Threading.Channels;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Tgs.GameInterface.Cards365.IntegrationTest;

namespace Tgs.GameInterface.Cards365.IntegrationTest;

public class PlayerLogoutUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://g5ky3i.365gaming.cc/");
		var website = "card365pega001";
		var cert = "40D237D0A995DFEAE0590F99552D3F6F";
		var extension1 = "cardp101";
		var playerCode = "4ucmqplxb029";
		var fakeService = Substitute.For<ICommandService<PlayerLogoutCommand>>();
		var web = new GameInterfaceCards365Application(builder => _ = builder.ConfigureServices(services => _ = services.AddTransient(_ => fakeService)));

		var sut = new Client.GameInterfaceCards365Client(new Protos.GameInterfaceCards365.GameInterfaceCards365Client(
				GrpcChannel.ForAddress(
					address: web.Server.BaseAddress,
					channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

		await sut.PlayerLogoutAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			extension1: extension1,
			playerCode: playerCode);

		_ = fakeService
			.Received(1)
			.ExecuteAsync(
			command: Arg.Is(new PlayerLogoutCommand(
				ApiUrl: apiUrl,
				Website: website,
				Cert: cert,
				Extension1: extension1,
				PlayerCode: playerCode)),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
