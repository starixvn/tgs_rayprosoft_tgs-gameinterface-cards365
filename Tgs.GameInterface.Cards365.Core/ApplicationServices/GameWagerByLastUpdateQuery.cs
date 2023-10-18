namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public record GameWagerByLastUpdateQuery(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Extension1,
    long Lastupdatedate);
