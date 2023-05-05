using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Core.Models;
using Tgs.GameInterface.Cards365.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Controllers;

[Route("[controller]")]
[ApiController]
public class Cards365Controller : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "playerCode": "4ucmqplxb029",
    ///             "transactionId": "9595e0fe83fe44819839bd07931738c9",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            Extension1: source.Extension1,
            PlayerCode: source.PlayerCode,
            TransactionId: source.TransactionId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            result.TransactionId,
            result.TsCode);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "playerCode": "4ucmqplxb029",
    ///             "transactionId": "6d2ae25ce6d54c10bed24b1beb1765ed",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            Extension1: source.Extension1,
            PlayerCode: source.PlayerCode,
            TransactionId: source.TransactionId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            result.TransactionId,
            result.TsCode);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "transactionId":"9595e0fe83fe44819839bd07931738c9"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            TransactionId: source.TransactionId);

        return service.GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Games the record by bet time asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "startTime": "2023-05-04T00:00:00+08:00",
    ///             "endTime": "2023-05-05T00:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("game/record/bet-time")]
    public async IAsyncEnumerable<GameRecordInfoViewModel> GameRecordByBetTimeAsync(
        [FromServices] IQueryService<GameRecordByBetTimeQuery, GameRecordInfo> service,
        GameRecordByBetTimeViewModel source)
    {
        var query = new GameRecordByBetTimeQuery(
            ApiUrl: source.ApiUrl,
                  Website: source.Website,
                  Cert: source.Cert,
                  Extension1: source.Extension1,
                  StartTime: source.StartTime,
                  EndTime: source.EndTime);

        var gameRecords = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameRecordInfoViewModel(
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalGameId: gameRecord.ExternalGameId,
                PlayerCode: gameRecord.PlayerCode,
                IsClosed: gameRecord.IsClosed,
                IsVoid: gameRecord.IsVoid,
                UpdateTime: gameRecord.UpdateTime,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                Validbet: gameRecord.Validbet,
                Settle: gameRecord.Settle,
                Winloss: gameRecord.Winloss,
                Commission: gameRecord.Commission,
                AdditionalData: gameRecord.AdditionalData);
        }
    }

    /// <summary>
    /// Games the record by last update asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "lastupdatedate": 0
    ///         }
    /// </remarks>
    [HttpPost("game/record/last-update")]
    public async IAsyncEnumerable<GameRecordInfoViewModel> GameRecordByLastUpdateAsync(
        [FromServices] IQueryService<GameRecordByLastUpdateQuery, GameRecordInfo> service,
        GameRecordByLastUpdateViewModel source)
    {
        var query = new GameRecordByLastUpdateQuery(
            ApiUrl: source.ApiUrl,
                  Website: source.Website,
                  Cert: source.Cert,
                  Extension1: source.Extension1,
                  Lastupdatedate: source.Lastupdatedate);

        var gameRecords = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameRecordInfoViewModel(
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalGameId: gameRecord.ExternalGameId,
                PlayerCode: gameRecord.PlayerCode,
                IsClosed: gameRecord.IsClosed,
                IsVoid: gameRecord.IsVoid,
                UpdateTime: gameRecord.UpdateTime,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                Validbet: gameRecord.Validbet,
                Settle: gameRecord.Settle,
                Winloss: gameRecord.Winloss,
                Commission: gameRecord.Commission,
                AdditionalData: gameRecord.AdditionalData);
        }
    }

    /// <summary>
    /// Games the record by pay time asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "startTime": "2023-05-04T00:00:00+08:00",
    ///             "endTime": "2023-05-05T00:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("game/record/pay-time")]
    public async IAsyncEnumerable<GameRecordInfoViewModel> GameRecordByPayTimeAsync(
        [FromServices] IQueryService<GameRecordByPayTimeQuery, GameRecordInfo> service,
        GameRecordByPayTimeViewModel source)
    {
        var query = new GameRecordByPayTimeQuery(
            ApiUrl: source.ApiUrl,
                  Website: source.Website,
                  Cert: source.Cert,
                  Extension1: source.Extension1,
                  StartTime: source.StartTime,
                  EndTime: source.EndTime);

        var gameRecords = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameRecordInfoViewModel(
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalGameId: gameRecord.ExternalGameId,
                PlayerCode: gameRecord.PlayerCode,
                IsClosed: gameRecord.IsClosed,
                IsVoid: gameRecord.IsVoid,
                UpdateTime: gameRecord.UpdateTime,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                Validbet: gameRecord.Validbet,
                Settle: gameRecord.Settle,
                Winloss: gameRecord.Winloss,
                Commission: gameRecord.Commission,
                AdditionalData: gameRecord.AdditionalData);
        }
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "playerCode": "4ucmqplxb029",
    ///             "currency": 3,
    ///             "language": "cn"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public ValueTask<string> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, string> service,
        GameUrlQueryViewModel source)
    {
        var query = new GameUrlQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            Extension1: source.Extension1,
            PlayerCode: source.PlayerCode,
            Currency: source.Currency,
            Language: source.Language);

        return service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            PlayerCode: source.PlayerCode);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "playerCode": "4ucmqplxb029",
    ///             "currency": 3
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            Extension1: source.Extension1,
            PlayerCode: source.PlayerCode,
            Currency: source.Currency);

        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the logout asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://g5ky3i.365gaming.cc/",
    ///             "website": "card365pega001",
    ///             "cert": "40D237D0A995DFEAE0590F99552D3F6F",
    ///             "extension1": "cardp101",
    ///             "playerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/logout", Name = "PlayerLogout")]
    public ValueTask PlayerLogoutAsync(
        [FromServices] ICommandService<PlayerLogoutCommand> service,
        PlayerLogoutViewModel source)
        => service
        .ExecuteAsync(
            command: new PlayerLogoutCommand(
                ApiUrl: source.ApiUrl,
                Website: source.Website,
                Cert: source.Cert,
                Extension1: source.Extension1,
                PlayerCode: source.PlayerCode),
            cancellationToken: HttpContext.RequestAborted);
}
