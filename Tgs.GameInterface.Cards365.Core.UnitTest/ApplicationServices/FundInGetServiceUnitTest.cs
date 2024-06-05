using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundInQuery(
			ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
			Website: "card365pega001",
			Cert: "40D237D0A995DFEAE0590F99552D3F6F",
			Extension1: "cardp101",
			PlayerCode: "4ucmqplxb029",
			TransactionId: "W1714538006874574800",
			Amount: 1);

		var withdrawResp = new WithdrawResp(Resp: "{\"status\":\"1\"}");
		_ = fakeGameApiClient.WithdrawAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			extension1: Arg.Is(query.Extension1),
			user: Arg.Is(query.PlayerCode),
			ts_code: Arg.Is(query.TransactionId.ToString()),
			amount: Arg.Is(query.Amount),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(withdrawResp);

		var service = new FundInGetService(fakeGameApiClient);

		var actual = await service.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(query.TransactionId, actual.TransactionId);
		Assert.Equal(withdrawResp.Resp, actual.Resp);
	}
}
