using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
			Website: "card365pega001",
			Cert: "40D237D0A995DFEAE0590F99552D3F6F",
			PlayerCode: "4ucmqplxb029");

		var result = new BalanceResp(
			IsPlayerExist: true,
			Balance: 0.0m);
		_ = fakeGameApiClient.GetBalanceAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			user: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var service = new PlayerBalanceGetService(fakeGameApiClient);

		var actual = await service.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(result.Balance, actual.Balance);
	}
}
