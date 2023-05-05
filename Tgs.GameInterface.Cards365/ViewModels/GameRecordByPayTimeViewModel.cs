namespace Tgs.GameInterface.Cards365.ViewModels;

public record GameRecordByPayTimeViewModel(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
