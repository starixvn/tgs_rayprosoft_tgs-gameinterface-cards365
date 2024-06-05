using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.GrpcServices;

public class GameInterfaceCards365GrpcService(
	ILogger<GameInterfaceCards365GrpcService> logger,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IQueryService<GameWagerByBetTimeQuery, GameWagerResult> gameWagerByBetTimeService,
	IQueryService<GameWagerByLastUpdateQuery, GameWagerResult> gameWagerByLastUpdateService,
	IQueryService<GameWagerByPayTimeQuery, GameWagerResult> gameWagerByPayTimeService,
	IGetService<GameUrlQuery, string> gameUrlService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	ICommandService<PlayerCreateCommand> playerCreateService,
	ICommandService<PlayerLogoutCommand> playerLogoutService,
	IGetService<PlayerOnlineQuery, bool> playerOnlineService) : GameInterfaceCards365.GameInterfaceCards365Base
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName
		});
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode,
			TransactionId: request.TransactionId,
			Amount: request.Amount);

		var result = await fundInService
			.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			TransactionId = result.TransactionId,
			Resp = result.Resp
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode,
			TransactionId: request.TransactionId,
			Amount: request.Amount);

		var result = await fundOutService.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			TransactionId = result.TransactionId,
			Resp = result.Resp
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			TransactionId: request.TransactionId);

		var result = await fundStatusService
			.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			State = result
		};
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency,
			Language: request.Language);

		var result = await gameUrlService
			.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply
		{
			GameUrl = result
		};
	}

	public override async Task<GameWagerReply> GameWagerByBetTime(GameWagerByBetTimeRequest request, ServerCallContext context)
	{
		var query = new GameWagerByBetTimeQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset());

		var gameWagers = await gameWagerByBetTimeService
			.QueryAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ToArrayAsync(
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		var reply = new GameWagerReply();

		if (gameWagers.Any())
		{
			foreach (var gameWager in gameWagers)
			{
				reply.Items.Add(new GameWagerInfo
				{
					ExternalRoundId = gameWager.ExternalRoundId,
					ExternalWagerId = gameWager.ExternalWagerId,
					ExternalGameId = gameWager.ExternalGameId,
					PlayerCode = gameWager.PlayerCode,
					IsClosed = gameWager.IsClosed,
					IsVoid = gameWager.IsVoid,
					UpdateTime = gameWager.UpdateTime.ToTimestamp(),
					ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
					Bet = gameWager.Bet,
					ValidBet = gameWager.Validbet,
					Settle = gameWager.Settle,
					Winloss = gameWager.Winloss,
					Commission = gameWager.Commission,
					AdditionalData = gameWager.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameWagerReply> GameWagerByLastUpdate(GameWagerByLastUpdateRequest request, ServerCallContext context)
	{
		var query = new GameWagerByLastUpdateQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			Lastupdatedate: request.Lastupdatedate);

		var gameWagers = await gameWagerByLastUpdateService
			.QueryAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ToArrayAsync(
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		var reply = new GameWagerReply();

		if (gameWagers.Any())
		{
			foreach (var gameWager in gameWagers)
			{
				reply.Items.Add(new GameWagerInfo
				{
					ExternalRoundId = gameWager.ExternalRoundId,
					ExternalWagerId = gameWager.ExternalWagerId,
					ExternalGameId = gameWager.ExternalGameId,
					PlayerCode = gameWager.PlayerCode,
					IsClosed = gameWager.IsClosed,
					IsVoid = gameWager.IsVoid,
					UpdateTime = gameWager.UpdateTime.ToTimestamp(),
					ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
					Bet = gameWager.Bet,
					ValidBet = gameWager.Validbet,
					Settle = gameWager.Settle,
					Winloss = gameWager.Winloss,
					Commission = gameWager.Commission,
					AdditionalData = gameWager.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<GameWagerReply> GameWagerByPayTime(GameWagerByPayTimeRequest request, ServerCallContext context)
	{
		var query = new GameWagerByPayTimeQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset());

		var gameWagers = await gameWagerByPayTimeService
			.QueryAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ToArrayAsync(
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<GameWagerResult>();

		var reply = new GameWagerReply();

		if (gameWagers.Any())
		{
			foreach (var gameWager in gameWagers)
			{
				reply.Items.Add(new GameWagerInfo
				{
					ExternalRoundId = gameWager.ExternalRoundId,
					ExternalWagerId = gameWager.ExternalWagerId,
					ExternalGameId = gameWager.ExternalGameId,
					PlayerCode = gameWager.PlayerCode,
					IsClosed = gameWager.IsClosed,
					IsVoid = gameWager.IsVoid,
					UpdateTime = gameWager.UpdateTime.ToTimestamp(),
					ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
					ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
					Bet = gameWager.Bet,
					ValidBet = gameWager.Validbet,
					Settle = gameWager.Settle,
					Winloss = gameWager.Winloss,
					Commission = gameWager.Commission,
					AdditionalData = gameWager.AdditionalData
				});
			}
		}

		return reply;
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			PlayerCode: request.PlayerCode);

		var result = await playerBalanceService
			.GetAsync(
				query: query,
				cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply
		{
			IsPlayerExist = result.IsPlayerExist,
			Balance = result.Balance
		};
	}

	public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
	{
		var command = new PlayerCreateCommand(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode,
			Currency: request.Currency);

		await playerCreateService
		   .ExecuteAsync(
			   command: command,
			   cancellationToken: context.CancellationToken)
		   .ConfigureAwait(false);

		return new BaseReply
		{
			State = true
		};
	}

	public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
	{
		var command = new PlayerLogoutCommand(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode);

		await playerLogoutService
		   .ExecuteAsync(
			   command: command,
			   cancellationToken: context.CancellationToken)
		   .ConfigureAwait(false);

		return new BaseReply
		{
			State = true
		};
	}

	public override async Task<PlayerOnlineReply> PlayerOnline(PlayerOnlineRequest request, ServerCallContext context)
	{
		var query = new PlayerOnlineQuery(
			ApiUrl: new(request.ApiUrl),
			Website: request.Website,
			Cert: request.Cert,
			Extension1: request.Extension1,
			PlayerCode: request.PlayerCode);

		var online = await playerOnlineService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerOnlineReply
		{
			Online = online
		};
	}
}
