using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundOutGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var resp = await _client.DepositAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            extension1: query.Extension1,
            user: query.PlayerCode,
            ts_code: query.TransactionId.ToString(),
            amount: query.Amount,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(
            TransactionId: query.TransactionId,
            Resp: resp.Resp);
    }
}
