namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;
public record PlayerOnlineQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string Extension1,
	string PlayerCode);
