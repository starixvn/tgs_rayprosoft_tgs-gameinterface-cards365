using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class FundStatusGetService(
    IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => client.GetBalanceOperationLogAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            ts_code: query.TransactionId,
            cancellationToken: cancellationToken);
}
