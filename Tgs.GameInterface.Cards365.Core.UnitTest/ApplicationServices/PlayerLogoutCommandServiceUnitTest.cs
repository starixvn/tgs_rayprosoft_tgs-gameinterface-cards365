using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerLogoutCommand(
			ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
			Website: "card365pega001",
			Cert: "40D237D0A995DFEAE0590F99552D3F6F",
			Extension1: "cardp101",
			PlayerCode: "4ucmqplxb029");

		var service = new PlayerLogoutCommandService(fakeGameApiClient);

		await service.ExecuteAsync(query);

		await fakeGameApiClient
			.Received(1)
			.LogoutAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			extension1: Arg.Is(query.Extension1),
			user: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
