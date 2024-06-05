namespace Tgs.GameInterface.Cards365.Core.Models;

public record TransactionInfo(
	string TxId,
	string GameNumber,
	int GameId,
	string UserId,
	string Status,
	DateTimeOffset UpdateTime,
	DateTimeOffset BetTransTime,
	DateTimeOffset PayTransTime,
	decimal Bet,
	decimal Validbet,
	decimal Winloss,
	decimal Commission,
	string AdditionalData);
