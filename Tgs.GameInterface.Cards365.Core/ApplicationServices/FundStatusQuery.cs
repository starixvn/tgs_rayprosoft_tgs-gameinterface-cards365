namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string TransactionId);
