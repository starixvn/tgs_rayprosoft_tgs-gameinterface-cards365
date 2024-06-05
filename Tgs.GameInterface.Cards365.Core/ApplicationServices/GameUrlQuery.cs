namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string Extension1,
	string PlayerCode,
	int Currency,
	string Language);
