using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class PlayerOnlineGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerOnlineQuery(
			ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
			Website: "card365pega001",
			Cert: "40D237D0A995DFEAE0590F99552D3F6F",
			Extension1: "cardp101",
			PlayerCode: "4ucmqplxb029");

		_ = fakeGameApiClient
			.GetUserStatusAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			extension1: Arg.Is(query.Extension1),
			user: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var service = new PlayerOnlineGetService(fakeGameApiClient);

		var actual = await service.GetAsync(query);

		Assert.True(actual);
	}
}
