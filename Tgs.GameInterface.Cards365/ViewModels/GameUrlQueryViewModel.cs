namespace Tgs.GameInterface.Cards365.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    string PlayerCode,
    int Currency,
    string Language);
