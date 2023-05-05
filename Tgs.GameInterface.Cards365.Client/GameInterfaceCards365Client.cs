using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Cards365.Protos;

namespace Tgs.GameInterface.Cards365.Client;

internal class GameInterfaceCards365Client : IGameInterfaceCards365Client
{
    private readonly GameInterfaceCards365.GameInterfaceCards365Client _client;

    public GameInterfaceCards365Client(GameInterfaceCards365.GameInterfaceCards365Client client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        };

        return await _client
            .FundInAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        };

        return await _client
            .FundOutAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string website,
        string cert,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            TransactionId = transactionId
        };

        var reply = await _client
            .FundStatusAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordByBetTimeAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameRecordByBetTimeRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
            .GameRecordByBetTimeAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordByLastUpdateAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        long lastupdatedate,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameRecordByLastUpdateRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            Lastupdatedate = lastupdatedate
        };

        var reply = await _client
            .GameRecordByLastUpdateAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordByPayTimeAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameRecordByPayTimeRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
            .GameRecordByPayTimeAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        string playerCode,
        int currency,
        string language,
        CancellationToken cancellationToken = default)
    {
        var request = new GameUrlRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode,
            Currency = currency,
            Language = language
        };

        var reply = await _client
            .GameUrlAsync(
                request: request,
                cancellationToken: cancellationToken)
           .ConfigureAwait(false);

        return reply.GameUrl;
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string website,
        string cert,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            PlayerCode = playerCode
        };

        return await _client
            .PlayerBalanceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        string playerCode,
        int currency,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerCreateRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode,
            Currency = currency
        };

        _ = await _client
            .PlayerCreateAsync(
                request: request,
                cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask PlayerLogout(
        Uri apiUrl,
        string website,
        string cert,
        string extension1,
        string playerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new PlayerLogoutRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode
        };

        _ = await _client
            .PlayerLogoutAsync(
               request: request,
               cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
