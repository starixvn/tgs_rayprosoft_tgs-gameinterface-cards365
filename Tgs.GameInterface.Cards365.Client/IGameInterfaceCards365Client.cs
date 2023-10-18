using Tgs.GameInterface.Cards365.Protos;

namespace Tgs.GameInterface.Cards365.Client;

public interface IGameInterfaceCards365Client
{
    ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(Uri apiUrl, string website, string cert, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, int currency, string language, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameWagerInfo> GameWagerByBetTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameWagerInfo> GameWagerByLastUpdateAsync(Uri apiUrl, string website, string cert, string extension1, long lastupdatedate, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameWagerInfo> GameWagerByPayTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, int currency, CancellationToken cancellationToken = default);

    ValueTask PlayerLogout(Uri apiUrl, string website, string cert, string extension1, string playerCode, CancellationToken cancellationToken = default);
}
