namespace Tgs.GameInterface.Cards365.ViewModels;

public record GameRecordInfoViewModel(
    string ExternalRoundId,
    string ExternalGameId,
    string PlayerCode,
    bool IsClosed,
    bool IsVoid,
    DateTimeOffset UpdateTime,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal Validbet,
    decimal Settle,
    decimal Winloss,
    decimal Commission,
    string AdditionalData);
