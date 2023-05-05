namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record GameRecordByPayTimeQuery(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
