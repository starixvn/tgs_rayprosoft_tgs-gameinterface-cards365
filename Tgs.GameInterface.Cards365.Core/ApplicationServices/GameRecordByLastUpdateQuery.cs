namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record GameRecordByLastUpdateQuery(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    long Lastupdatedate);
