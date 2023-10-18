using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.Cards365.Core.Exceptions;
using Tgs.GameInterface.Cards365.Core.Extensions;
using Tgs.GameInterface.Cards365.Core.Models;

namespace Tgs.GameInterface.Cards365.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<DepositResp> DepositAsync(Uri apiUrl, string website, string cert, string extension1, string user, string ts_code, decimal amount, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "balance",amount.ToString()},
            { "ts_code",ts_code},
            { "extension1",extension1}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/deposit");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "deposit",
                cancellationToken: cancellationToken);

        return !jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsond["status"]!.GetValue<string>())
            : new DepositResp(jsond.ToJsonString());
    }

    public async ValueTask<BalanceResp> GetBalanceAsync(Uri apiUrl, string website, string cert, string user, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "alluser","0"},
            { "users",user}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getBalance");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getBalance",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            return jsond["status"]!.GetValue<string>().Equals("1032", StringComparison.OrdinalIgnoreCase)
                ? new BalanceResp(false, 0)
                : throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }

        var results = jsond["results"]!.Deserialize<IEnumerable<BalanceInfo>>() ?? Enumerable.Empty<BalanceInfo>();

        if (!results.Any())
        {
            return new BalanceResp(false, 0);
        }

        var result = results.Single(x => x.User.Equals(user, StringComparison.OrdinalIgnoreCase));

        return new BalanceResp(true, result.Balance);
    }

    public async ValueTask<bool> GetBalanceOperationLogAsync(Uri apiUrl, string website, string cert, string ts_code, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "ts_code",ts_code}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getBalanceOperationLog");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getBalanceOperationLog",
                cancellationToken: cancellationToken);

        return !jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsond["status"]!.GetValue<string>())
            : jsond["result"]!.AsArray().Any();
    }

    public async IAsyncEnumerable<BalanceInfo> GetBalancesAsync(Uri apiUrl, string website, string cert, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "alluser","1"}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getBalance");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getBalances",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }

        var results = jsond["results"]!.Deserialize<IEnumerable<BalanceInfo>>() ?? Enumerable.Empty<BalanceInfo>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            if (result.Balance == 0)
            {
                continue;
            }
            yield return result;
        }
    }

    public async ValueTask<string> GetKeyAsync(Uri apiUrl, string website, string cert, string extension1, string user, string userName, int currency, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "userName",userName},
            { "extension1",extension1},
            { "currency",currency.ToString()}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getKey");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getKey",
                cancellationToken: cancellationToken);

        return !jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsond["status"]!.GetValue<string>())
            : jsond["key"]!.GetValue<string>();
    }

    public async IAsyncEnumerable<TransactionInfo> GetTransactionsByBetTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "extension1",extension1},
            { "status",0.ToString()},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getTransactionsByBetTime");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getTransactionsByBetTime",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            if (jsond["status"]!.GetValue<string>().Equals("1003", StringComparison.OrdinalIgnoreCase))
            {
                yield break;
            }
            throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }

        var transactions = jsond["transactions"]!.AsArray();

        if (!transactions.Any())
        {
            yield break;
        }

        foreach (var result in transactions)
        {
            if (result is null)
            {
                continue;
            }

            yield return new TransactionInfo(
                TxId: result["txId"]!.GetValue<string>(),
                GameNumber: result["gameNumber"]!.GetValue<string>(),
                GameId: result["gameId"]!.GetValue<int>(),
                UserId: result["userId"]!.GetValue<string>(),
                Status: result["status"]!.GetValue<string>(),
                UpdateTime: result["updateTime"]!.GetValue<string>().StringConvertToDatetime(),
                BetTransTime: result["betTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                PayTransTime: result["payTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                Bet: result["realBetAmount"]!.GetValue<decimal>(),
                Validbet: result["validbet"]!.GetValue<decimal>(),
                Winloss: result["profit"]!.GetValue<decimal>(),
                Commission: result["commission"]!.GetValue<decimal>(),
                AdditionalData: result.ToJsonString());
        }
    }

    public async IAsyncEnumerable<TransactionInfo> GetTransactionsByLastUpdateDateAsync(Uri apiUrl, string website, string cert, string extension1, long lastupdatedate, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "extension1",extension1},
            { "status","-1"},
            { "lastupdatedate",lastupdatedate.ToString()}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getTransactionsByLastUpdateDate");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getTransactionsByLastUpdateDate",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            if (jsond["status"]!.GetValue<string>().Equals("1003", StringComparison.OrdinalIgnoreCase))
            {
                yield break;
            }
            throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }

        var transactions = jsond["transactions"]!.AsArray();

        if (!transactions.Any())
        {
            yield break;
        }

        foreach (var result in transactions)
        {
            if (result is null)
            {
                continue;
            }

            yield return new TransactionInfo(
                TxId: result["txId"]!.GetValue<string>(),
                GameNumber: result["gameNumber"]!.GetValue<string>(),
                GameId: result["gameId"]!.GetValue<int>(),
                UserId: result["userId"]!.GetValue<string>(),
                Status: result["status"]!.GetValue<string>(),
                UpdateTime: result["updateTime"]!.GetValue<string>().StringConvertToDatetime(),
                BetTransTime: result["betTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                PayTransTime: result["payTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                Bet: result["realBetAmount"]!.GetValue<decimal>(),
                Validbet: result["validbet"]!.GetValue<decimal>(),
                Winloss: result["profit"]!.GetValue<decimal>(),
                Commission: result["commission"]!.GetValue<decimal>(),
                AdditionalData: result.ToJsonString());
        }
    }

    public async IAsyncEnumerable<TransactionInfo> GetTransactionsByPayTimeAsync(Uri apiUrl, string website, string cert, string extension1, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "extension1",extension1},
            { "status","-1"},
            { "st",startTime.ToUnixTimeMilliseconds().ToString()},
            { "et",endTime.ToUnixTimeMilliseconds().ToString()}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/getTransactionsByPayTime");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "getTransactionsByPayTime",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            if (jsond["status"]!.GetValue<string>().Equals("1003", StringComparison.OrdinalIgnoreCase))
            {
                yield break;
            }
            throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }

        var transactions = jsond["transactions"]!.AsArray();

        if (!transactions.Any())
        {
            yield break;
        }

        foreach (var result in transactions)
        {
            if (result is null)
            {
                continue;
            }

            yield return new TransactionInfo(
                TxId: result["txId"]!.GetValue<string>(),
                GameNumber: result["gameNumber"]!.GetValue<string>(),
                GameId: result["gameId"]!.GetValue<int>(),
                UserId: result["userId"]!.GetValue<string>(),
                Status: result["status"]!.GetValue<string>(),
                UpdateTime: result["updateTime"]!.GetValue<string>().StringConvertToDatetime(),
                BetTransTime: result["betTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                PayTransTime: result["payTransTime"]!.GetValue<string>().StringConvertToDatetime(),
                Bet: result["realBetAmount"]!.GetValue<decimal>(),
                Validbet: result["validbet"]!.GetValue<decimal>(),
                Winloss: result["profit"]!.GetValue<decimal>(),
                Commission: result["commission"]!.GetValue<decimal>(),
                AdditionalData: result.ToJsonString());
        }
    }

    public async ValueTask LogoutAsync(Uri apiUrl, string website, string cert, string extension1, string user, CancellationToken cancellationToken = default)

    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "extension1",extension1}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/logout");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "logout",
                cancellationToken: cancellationToken);

        if (!jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameInterfaceException(jsond["status"]!.GetValue<string>());
        }
    }

    public async ValueTask<WithdrawResp> WithdrawAsync(Uri apiUrl, string website, string cert, string extension1, string user, string ts_code, decimal amount, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            { "cert",cert},
            { "user",user},
            { "withdrawtype","1"},
            { "balance",amount.ToString()},
            { "ts_code",ts_code},
            { "extension1",extension1}
        };

        apiUrl = new Uri(apiUrl, $"/api/{website}/withdraw");

        var jsond = await PostAsync(
                apiUrl: apiUrl,
                parameters: parameters,
                logPrefix: "withdraw",
                cancellationToken: cancellationToken);

        return !jsond["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsond["status"]!.GetValue<string>())
            : new WithdrawResp(jsond.ToJsonString());
    }

    private async ValueTask<JsonNode> PostAsync(Uri apiUrl, IDictionary<string, string> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var content = new FormUrlEncodedContent(parameters);
        var httpResponseMessage = await _client
            .PostAsync(
                requestUri: apiUrl,
                content: content,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
