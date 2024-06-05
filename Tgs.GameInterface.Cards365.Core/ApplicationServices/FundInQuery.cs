namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record FundInQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string Extension1,
	string PlayerCode,
	string TransactionId,
	decimal Amount);
