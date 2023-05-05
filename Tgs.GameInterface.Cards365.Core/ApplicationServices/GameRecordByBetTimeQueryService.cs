using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameRecordByBetTimeQueryService : IQueryService<GameRecordByBetTimeQuery, GameRecordInfo>
{
    private readonly IGameApiClient _client;
    private readonly IGameRecordUtensil _gameRecordUtensil;

    public GameRecordByBetTimeQueryService(
        IGameApiClient client,
        IGameRecordUtensil gameRecordUtensil)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gameRecordUtensil = gameRecordUtensil ?? throw new ArgumentNullException(nameof(gameRecordUtensil));
    }

    public async IAsyncEnumerable<GameRecordInfo> QueryAsync(
        GameRecordByBetTimeQuery query,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client
              .GetTransactionsByBetTimeAsync(
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
            yield return _gameRecordUtensil.TransactionConvertGameRecord(transaction);
        }
    }
}
