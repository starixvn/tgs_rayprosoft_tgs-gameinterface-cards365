using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameRecordByLastUpdateQueryService : IQueryService<GameRecordByLastUpdateQuery, GameRecordInfo>
{
    private readonly IGameApiClient _client;
    private readonly IGameRecordUtensil _gameRecordUtensil;

    public GameRecordByLastUpdateQueryService(
        IGameApiClient client,
        IGameRecordUtensil gameRecordUtensil)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gameRecordUtensil = gameRecordUtensil ?? throw new ArgumentNullException(nameof(gameRecordUtensil));
    }

    public async IAsyncEnumerable<GameRecordInfo> QueryAsync(
        GameRecordByLastUpdateQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client
              .GetTransactionsByLastUpdateDateAsync(
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
            yield return _gameRecordUtensil.TransactionConvertGameRecord(transaction);
        }
    }
}
