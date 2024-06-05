using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core;

public interface IGameWagerUtensil
{
	GameWagerResult TransactionConvertGameWager(TransactionInfo source);
}
