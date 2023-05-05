using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class GameRecordByLastUpdateQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();
        var fakeGameRecordUtensil = Substitute.For<IGameRecordUtensil>();

        var query = new GameRecordByLastUpdateQuery(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            Extension1: "cardp101",
            Lastupdatedate: 0);

        var client = new GameRecordByLastUpdateQueryService(
            fakeGameApiClient,
            fakeGameRecordUtensil);

        _ = await client
           .QueryAsync(
              query)
           .ToArrayAsync()
           .ConfigureAwait(false)
           ?? Enumerable.Empty<GameRecordInfo>();

        _ = await fakeGameApiClient
             .Received(1)
             .GetTransactionsByLastUpdateDateAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 website: Arg.Is(query.Website),
                 cert: Arg.Is(query.Cert),
                 extension1: Arg.Is(query.Extension1),
                 lastupdatedate: Arg.Is(query.Lastupdatedate),
                 cancellationToken: Arg.Any<CancellationToken>())
             .ToArrayAsync()
             .ConfigureAwait(false)
             ?? Enumerable.Empty<TransactionInfo>();
    }
}
