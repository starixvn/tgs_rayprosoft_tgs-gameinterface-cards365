namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    string PlayerCode);
