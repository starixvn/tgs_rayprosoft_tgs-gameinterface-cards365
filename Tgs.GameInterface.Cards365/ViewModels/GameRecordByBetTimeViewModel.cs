namespace Tgs.GameInterface.Cards365.ViewModels;

public record GameRecordByBetTimeViewModel(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
