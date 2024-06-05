namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string PlayerCode);
