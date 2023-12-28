using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameWagerByPayTimeQueryService(
    IGameApiClient client,
    IGameWagerUtensil gameWagerUtensil) : IQueryService<GameWagerByPayTimeQuery, GameWagerResult>
{
    public async IAsyncEnumerable<GameWagerResult> QueryAsync(
        GameWagerByPayTimeQuery query,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await client.GetTransactionsByPayTimeAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            extension1: query.Extension1,
            startTime: query.StartTime,
            endTime: query.EndTime,
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
            yield return gameWagerUtensil.TransactionConvertGameWager(transaction);
        }
    }
}
