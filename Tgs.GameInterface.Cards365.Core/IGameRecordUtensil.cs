using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core;

public interface IGameRecordUtensil
{
    GameRecordInfo TransactionConvertGameRecord(TransactionInfo source);
}
