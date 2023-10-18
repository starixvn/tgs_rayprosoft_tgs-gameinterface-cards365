using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            TransactionId: Guid.NewGuid().ToString("N"));

        _ = fakeGameApiClient.GetBalanceOperationLogAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            ts_code: Arg.Is(query.TransactionId),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(true);

        var service = new FundStatusGetService(fakeGameApiClient);

        var actual = await service.GetAsync(query);

        Assert.True(actual);
    }
}
