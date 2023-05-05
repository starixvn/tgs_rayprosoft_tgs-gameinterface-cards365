namespace Tgs.GameInterface.Cards365.ViewModels;

public record PlayerLogoutViewModel(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    string PlayerCode);
