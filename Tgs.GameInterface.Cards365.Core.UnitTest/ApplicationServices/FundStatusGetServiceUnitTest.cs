using System.Text;
using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeTransactionUtensil = Substitute.For<ITransactionUtensil>();
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            TransactionId: Guid.NewGuid().ToString("N"));

        var transactionId = Convert.ToBase64String(Encoding.UTF8.GetBytes(query.TransactionId));
        _ = fakeTransactionUtensil
            .TransactionIdConvertBase64(
                Arg.Is(query.TransactionId))
            .Returns(transactionId);

        _ = fakeGameApiClient
             .GetBalanceOperationLogAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 website: Arg.Is(query.Website),
                 cert: Arg.Is(query.Cert),
                 ts_code: Arg.Is(transactionId),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(true);

        var client = new FundStatusGetService(
            fakeTransactionUtensil,
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.True(actual);
    }
}
