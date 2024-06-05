using System.Runtime.CompilerServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameWagerByLastUpdateQueryService(
	IGameApiClient client,
	IGameWagerUtensil gameWagerUtensil) : IQueryService<GameWagerByLastUpdateQuery, GameWagerResult>
{
	public async IAsyncEnumerable<GameWagerResult> QueryAsync(
		GameWagerByLastUpdateQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var transactions = await client.GetTransactionsByLastUpdateDateAsync(
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
			yield return gameWagerUtensil.TransactionConvertGameWager(transaction);
		}
	}
}
