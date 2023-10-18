using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core;

public interface IGameApiClient
{
    ValueTask<DepositResp> DepositAsync(Uri apiUrl, string website, string cert, string extension1, string user, string ts_code, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<BalanceResp> GetBalanceAsync(Uri apiUrl, string website, string cert, string user, CancellationToken cancellationToken = default);

    ValueTask<bool> GetBalanceOperationLogAsync(Uri apiUrl, string website, string cert, string ts_code, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BalanceInfo> GetBalancesAsync(Uri apiUrl, string website, string cert, CancellationToken cancellationToken = default);

    ValueTask<string> GetKeyAsync(Uri apiUrl, string website, string cert, string extension1, string user, string userName, int currency, CancellationToken cancellationToken = default);

    IAsyncEnumerable<TransactionInfo> GetTransactionsByBetTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

    IAsyncEnumerable<TransactionInfo> GetTransactionsByLastUpdateDateAsync(Uri apiUrl, string website, string cert, string extension1, long lastupdatedate, CancellationToken cancellationToken = default);

    IAsyncEnumerable<TransactionInfo> GetTransactionsByPayTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(Uri apiUrl, string website, string cert, string extension1, string user, CancellationToken cancellationToken = default);

    ValueTask<WithdrawResp> WithdrawAsync(Uri apiUrl, string website, string cert, string extension1, string user, string ts_code, decimal amount, CancellationToken cancellationToken = default);
}
