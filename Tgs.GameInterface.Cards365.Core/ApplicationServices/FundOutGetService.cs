using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;
    private readonly ITransactionUtensil _transactionUtensil;

    public FundOutGetService(
        ITransactionUtensil transactionUtensil,
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _transactionUtensil = transactionUtensil ?? throw new ArgumentNullException(nameof(transactionUtensil));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var ts_code = _transactionUtensil.TransactionIdConvertBase64(query.TransactionId);

        await _client.DepositAsync(
                apiUrl: query.ApiUrl,
                website: query.Website,
                cert: query.Cert,
                extension1: query.Extension1,
                user: query.PlayerCode,
                ts_code: ts_code.ToString(),
                amount: query.Amount,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TransactionId: query.TransactionId,
            TsCode: ts_code);
    }
}
