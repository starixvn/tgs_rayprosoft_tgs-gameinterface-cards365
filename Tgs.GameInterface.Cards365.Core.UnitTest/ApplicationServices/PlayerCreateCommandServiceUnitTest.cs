using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerCreateCommand(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            Extension1: "cardp101",
            PlayerCode: "4ucmqplxb029",
            Currency: 3);

        var client = new PlayerCreateCommandService(
            fakeGameApiClient);
        await client
            .ExecuteAsync(
                query)
            .ConfigureAwait(false);

        _ = await fakeGameApiClient
            .Received(1)
            .GetKeyAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                website: Arg.Is(query.Website),
                cert: Arg.Is(query.Cert),
                extension1: Arg.Is(query.Extension1),
                user: Arg.Is(query.PlayerCode),
                userName: Arg.Is(query.PlayerCode),
                currency: Arg.Is(query.Currency),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
