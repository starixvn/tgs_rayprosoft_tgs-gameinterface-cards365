using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameRecordByPayTimeQueryService : IQueryService<GameRecordByPayTimeQuery, GameRecordInfo>
{
    private readonly IGameApiClient _client;
    private readonly IGameRecordUtensil _gameRecordUtensil;

    public GameRecordByPayTimeQueryService(
        IGameApiClient client,
        IGameRecordUtensil gameRecordUtensil)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _gameRecordUtensil = gameRecordUtensil ?? throw new ArgumentNullException(nameof(gameRecordUtensil));
    }

    public async IAsyncEnumerable<GameRecordInfo> QueryAsync(
        GameRecordByPayTimeQuery query,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transactions = await _client
              .GetTransactionsByPayTimeAsync(
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
