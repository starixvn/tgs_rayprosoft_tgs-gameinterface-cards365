using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class GameWagerByPayTimeQueryServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		var fakeGameWagerUtensil = Substitute.For<IGameWagerUtensil>();

		var query = new GameWagerByPayTimeQuery(
			ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
			Website: "card365pega001",
			Cert: "40D237D0A995DFEAE0590F99552D3F6F",
			Extension1: "cardp101",
			StartTime: DateTimeOffset.UtcNow.AddMinutes(-10),
			EndTime: DateTimeOffset.UtcNow);

		var service = new GameWagerByPayTimeQueryService(
			fakeGameApiClient,
			fakeGameWagerUtensil);

		_ = await service.QueryAsync(
			query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameWagerResult>();

		_ = await fakeGameApiClient
			.Received(1)
			.GetTransactionsByPayTimeAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			extension1: Arg.Is(query.Extension1),
			startTime: Arg.Is(query.StartTime),
			endTime: Arg.Is(query.EndTime),
			cancellationToken: Arg.Any<CancellationToken>())
			.ToArrayAsync()
			?? Enumerable.Empty<TransactionInfo>();
	}
}
