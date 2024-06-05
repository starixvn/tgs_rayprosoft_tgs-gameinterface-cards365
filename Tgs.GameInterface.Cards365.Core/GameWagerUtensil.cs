using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core;

internal class GameWagerUtensil : IGameWagerUtensil
{
	public GameWagerResult TransactionConvertGameWager(TransactionInfo source)
		=> new(
			ExternalRoundId: source.GameNumber,
			ExternalWagerId: source.TxId,
			ExternalGameId: source.GameId.ToString(),
			PlayerCode: source.UserId,
			IsClosed: !source.Status.Equals("Bet", StringComparison.OrdinalIgnoreCase),
			IsVoid: source.Status.Equals("Void", StringComparison.OrdinalIgnoreCase),
			UpdateTime: source.UpdateTime,
			ExternalCreatedOn: source.BetTransTime,
			ExternalClosedOn: source.PayTransTime,
			Bet: source.Bet,
			Validbet: source.Validbet,
			Settle: source.Status.Equals("Void", StringComparison.OrdinalIgnoreCase) ? 0.0m : source.Winloss + source.Validbet,
			Winloss: source.Winloss,
			Commission: source.Commission,
			AdditionalData: source.AdditionalData);
}
