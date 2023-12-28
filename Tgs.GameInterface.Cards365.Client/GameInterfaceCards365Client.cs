using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Cards365.Protos;

namespace Tgs.GameInterface.Cards365.Client;

internal class GameInterfaceCards365Client(
    GameInterfaceCards365.GameInterfaceCards365Client client) : IGameInterfaceCards365Client
{
    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
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

        return await client.FundInAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
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

        return await client.FundOutAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string website, string cert, string transactionId, CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            TransactionId = transactionId
        };

        var reply = await client.FundStatusAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.State;
    }

    public async ValueTask<string> GameUrlAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, int currency, string language, CancellationToken cancellationToken = default)
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

        var reply = await client
            .GameUrlAsync(
                request: request,
                cancellationToken: cancellationToken)
           .ConfigureAwait(false);

        return reply.GameUrl;
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerByBetTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameWagerByBetTimeRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await client.GameWagerByBetTimeAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameWagers = reply.Items
            ?? Enumerable.Empty<GameWagerInfo>();

        if (!gameWagers.Any())
        {
            yield break;
        }

        foreach (var gameWager in gameWagers)
        {
            yield return gameWager;
        }
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerByLastUpdateAsync(Uri apiUrl, string website, string cert, string extension1, long lastupdatedate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameWagerByLastUpdateRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            Lastupdatedate = lastupdatedate
        };

        var reply = await client.GameWagerByLastUpdateAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameWagers = reply.Items
            ?? Enumerable.Empty<GameWagerInfo>();

        if (!gameWagers.Any())
        {
            yield break;
        }

        foreach (var gameWager in gameWagers)
        {
            yield return gameWager;
        }
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerByPayTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameWagerByPayTimeRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await client.GameWagerByPayTimeAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameWagers = reply.Items
            ?? Enumerable.Empty<GameWagerInfo>();

        if (!gameWagers.Any())
        {
            yield break;
        }

        foreach (var gameWager in gameWagers)
        {
            yield return gameWager;
        }
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default)
    {
        var request = new PlayerBalanceRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            PlayerCode = playerCode
        };

        return await client
            .PlayerBalanceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(Uri apiUrl, string website, string cert, string extension1, string playerCode, int currency, CancellationToken cancellationToken = default)
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

        _ = await client
            .PlayerCreateAsync(
                request: request,
                cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask PlayerLogout(Uri apiUrl, string website, string cert, string extension1, string playerCode, CancellationToken cancellationToken = default)
    {
        var request = new PlayerLogoutRequest
        {
            ApiUrl = apiUrl.ToString(),
            Website = website,
            Cert = cert,
            Extension1 = extension1,
            PlayerCode = playerCode
        };

        _ = await client
            .PlayerLogoutAsync(
               request: request,
               cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
