using NSubstitute;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;

namespace Tgs.GameInterface.Cards365.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeTransactionUtensil = Substitute.For<ITransactionUtensil>();
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundInQuery(
            ApiUrl: new Uri("https://g5ky3i.365gaming.cc/"),
            Website: "card365pega001",
            Cert: "40D237D0A995DFEAE0590F99552D3F6F",
            Extension1: "cardp101",
            PlayerCode: "4ucmqplxb029",
            TransactionId: "9595e0fe83fe44819839bd07931738c9",
            Amount: 1);

        var ts_code = "_uCVlf6DgUSYOb0Hkxc4yQ";
        _ = fakeTransactionUtensil
            .TransactionIdConvertBase64(
                Arg.Is(query.TransactionId))
            .Returns(ts_code);

        var client = new FundInGetService(
            fakeTransactionUtensil,
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(query.TransactionId, actual.TransactionId);
        Assert.Equal(ts_code, actual.TsCode);

        await fakeGameApiClient
            .Received(1)
            .WithdrawAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                website: Arg.Is(query.Website),
                cert: Arg.Is(query.Cert),
                extension1: Arg.Is(query.Extension1),
                user: Arg.Is(query.PlayerCode),
                ts_code: Arg.Is(ts_code.ToString()),
                amount: Arg.Is(query.Amount),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);

        await fakeGameApiClient
            .Received(0)
            .DepositAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                website: Arg.Is(query.Website),
                cert: Arg.Is(query.Cert),
                extension1: Arg.Is(query.Extension1),
                user: Arg.Is(query.PlayerCode),
                ts_code: Arg.Is(ts_code.ToString()),
                amount: Arg.Is(query.Amount),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
