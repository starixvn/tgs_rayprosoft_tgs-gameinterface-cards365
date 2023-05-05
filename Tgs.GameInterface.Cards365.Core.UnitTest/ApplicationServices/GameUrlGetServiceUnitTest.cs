using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            Extension1: "cardp101",
            PlayerCode: "4ucmqplxb029",
            Currency: 3,
            Language: "cn");

        var key = Guid.NewGuid().ToString("N");
        _ = fakeGameApiClient
             .GetKeyAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                website: Arg.Is(query.Website),
                cert: Arg.Is(query.Cert),
                extension1: Arg.Is(query.Extension1),
                user: Arg.Is(query.PlayerCode),
                userName: Arg.Is(query.PlayerCode),
                currency: Arg.Is(query.Currency),
                cancellationToken: Arg.Any<CancellationToken>())
             .Returns(key);

        var client = new GameUrlGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal($"https://g5ky3i.365gaming.cc:443/api/card365pega001/loginV2?user=4ucmqplxb029&key={key}&extension1=cardp101&userName=4ucmqplxb029&language=cn&fullscreen=0&jackpot=0&blockfree=1", actual);
    }
}
