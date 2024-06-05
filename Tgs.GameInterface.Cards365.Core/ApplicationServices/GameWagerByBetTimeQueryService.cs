using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameWagerByBetTimeQueryService(
	IGameApiClient client,
	IGameWagerUtensil gameWagerUtensil) : IQueryService<GameWagerByBetTimeQuery, GameWagerResult>
{
	public async IAsyncEnumerable<GameWagerResult> QueryAsync(
		GameWagerByBetTimeQuery query,
	   [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var transactions = await client.GetTransactionsByBetTimeAsync(
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
