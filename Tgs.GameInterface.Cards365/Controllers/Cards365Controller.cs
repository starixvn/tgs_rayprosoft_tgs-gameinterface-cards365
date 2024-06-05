using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
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
	///             "transactionId": "W1714538006874574800",
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

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			result.TransactionId,
			result.Resp);
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
	///             "transactionId": "D1714538127095910400",
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

		var result = await service.GetAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false);

		return new FundResultViewModel(
			result.TransactionId,
			result.Resp);
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
	///             "transactionId":"W1714538006874574800"
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
	[HttpPost("game/wager/bet-time")]
	public async IAsyncEnumerable<GameWagerInfoViewModel> GameWagerByBetTimeAsync(
		[FromServices] IQueryService<GameWagerByBetTimeQuery, GameWagerResult> service,
		GameWagerByBetTimeViewModel source)
	{
		var query = new GameWagerByBetTimeQuery(
			ApiUrl: source.ApiUrl,
			Website: source.Website,
			Cert: source.Cert,
			Extension1: source.Extension1,
			StartTime: source.StartTime,
			EndTime: source.EndTime);

		var gameWagers = await service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		if (!gameWagers.Any())
		{
			yield break;
		}

		foreach (var gameWager in gameWagers)
		{
			yield return new GameWagerInfoViewModel(
				ExternalRoundId: gameWager.ExternalRoundId,
				ExternalWagerId: gameWager.ExternalWagerId,
				ExternalGameId: gameWager.ExternalGameId,
				PlayerCode: gameWager.PlayerCode,
				IsClosed: gameWager.IsClosed,
				IsVoid: gameWager.IsVoid,
				UpdateTime: gameWager.UpdateTime,
				ExternalCreatedOn: gameWager.ExternalCreatedOn,
				ExternalClosedOn: gameWager.ExternalClosedOn,
				Bet: gameWager.Bet,
				Validbet: gameWager.Validbet,
				Settle: gameWager.Settle,
				Winloss: gameWager.Winloss,
				Commission: gameWager.Commission,
				AdditionalData: gameWager.AdditionalData);
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
	[HttpPost("game/wager/last-update")]
	public async IAsyncEnumerable<GameWagerInfoViewModel> GameWagerByLastUpdateAsync(
		[FromServices] IQueryService<GameWagerByLastUpdateQuery, GameWagerResult> service,
		GameWagerByLastUpdateViewModel source)
	{
		var query = new GameWagerByLastUpdateQuery(
			ApiUrl: source.ApiUrl,
			Website: source.Website,
			Cert: source.Cert,
			Extension1: source.Extension1,
			Lastupdatedate: source.Lastupdatedate);

		var gameWagers = await service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		if (!gameWagers.Any())
		{
			yield break;
		}

		foreach (var gameWager in gameWagers)
		{
			yield return new GameWagerInfoViewModel(
				ExternalRoundId: gameWager.ExternalRoundId,
				ExternalWagerId: gameWager.ExternalWagerId,
				ExternalGameId: gameWager.ExternalGameId,
				PlayerCode: gameWager.PlayerCode,
				IsClosed: gameWager.IsClosed,
				IsVoid: gameWager.IsVoid,
				UpdateTime: gameWager.UpdateTime,
				ExternalCreatedOn: gameWager.ExternalCreatedOn,
				ExternalClosedOn: gameWager.ExternalClosedOn,
				Bet: gameWager.Bet,
				Validbet: gameWager.Validbet,
				Settle: gameWager.Settle,
				Winloss: gameWager.Winloss,
				Commission: gameWager.Commission,
				AdditionalData: gameWager.AdditionalData);
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
	[HttpPost("game/wager/pay-time")]
	public async IAsyncEnumerable<GameWagerInfoViewModel> GameWagerByPayTimeAsync(
		[FromServices] IQueryService<GameWagerByPayTimeQuery, GameWagerResult> service,
		GameWagerByPayTimeViewModel source)
	{
		var query = new GameWagerByPayTimeQuery(
			ApiUrl: source.ApiUrl,
			Website: source.Website,
			Cert: source.Cert,
			Extension1: source.Extension1,
			StartTime: source.StartTime,
			EndTime: source.EndTime);

		var gameWagers = await service.QueryAsync(
			query: query,
			cancellationToken: HttpContext.RequestAborted)
			.ToArrayAsync(
			cancellationToken: HttpContext.RequestAborted)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		if (!gameWagers.Any())
		{
			yield break;
		}

		foreach (var gameWager in gameWagers)
		{
			yield return new GameWagerInfoViewModel(
				ExternalRoundId: gameWager.ExternalRoundId,
				ExternalWagerId: gameWager.ExternalWagerId,
				ExternalGameId: gameWager.ExternalGameId,
				PlayerCode: gameWager.PlayerCode,
				IsClosed: gameWager.IsClosed,
				IsVoid: gameWager.IsVoid,
				UpdateTime: gameWager.UpdateTime,
				ExternalCreatedOn: gameWager.ExternalCreatedOn,
				ExternalClosedOn: gameWager.ExternalClosedOn,
				Bet: gameWager.Bet,
				Validbet: gameWager.Validbet,
				Settle: gameWager.Settle,
				Winloss: gameWager.Winloss,
				Commission: gameWager.Commission,
				AdditionalData: gameWager.AdditionalData);
		}
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

		var result = await service.GetAsync(
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
		=> service.ExecuteAsync(
			command: new PlayerLogoutCommand(
				ApiUrl: source.ApiUrl,
				Website: source.Website,
				Cert: source.Cert,
				Extension1: source.Extension1,
				PlayerCode: source.PlayerCode),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the online.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="query">The query.</param>
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
	[HttpPost("player/online", Name = "PlayerOnline")]
	public ValueTask<bool> PlayerOnline(
		[FromServices] IGetService<PlayerOnlineQuery, bool> service,
		[FromBody] PlayerOnlineQuery query)
		=> service.GetAsync(query);
}
