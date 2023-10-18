using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameWagerByLastUpdateQueryService : IQueryService<GameWagerByLastUpdateQuery, GameWagerResult>
{
    private readonly IGameApiClient _client;
    private readonly IGameWagerUtensil _gameWagerUtensil;

    public GameWagerByLastUpdateQueryService(
        IGameApiClient client,
        IGameWagerUtensil gameWagerUtensil)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gameWagerUtensil = gameWagerUtensil ?? throw new ArgumentNullException(nameof(gameWagerUtensil));
    }

    public async IAsyncEnumerable<GameWagerResult> QueryAsync(
        GameWagerByLastUpdateQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client.GetTransactionsByLastUpdateDateAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            extension1: query.Extension1,
            lastupdatedate: query.Lastupdatedate,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<TransactionInfo>();

        if (!transactions.Any())
        {
            yield break;
        }

        foreach (var transaction in transactions)
        {
            yield return _gameWagerUtensil.TransactionConvertGameWager(transaction);
        }
    }
}
