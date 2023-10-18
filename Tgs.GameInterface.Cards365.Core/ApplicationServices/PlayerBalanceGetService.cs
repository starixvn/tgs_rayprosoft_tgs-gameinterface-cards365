using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.GetBalanceAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            user: query.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult(
            result.IsPlayerExist,
            result.Balance);
    }
}
