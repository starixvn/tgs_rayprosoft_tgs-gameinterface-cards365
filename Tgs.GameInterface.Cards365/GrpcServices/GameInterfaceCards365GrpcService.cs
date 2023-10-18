using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Cards365.Core.ApplicationServices;
using Tgs.GameInterface.Cards365.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.GrpcServices
{
    public class GameInterfaceCards365GrpcService : GameInterfaceCards365.GameInterfaceCards365Base
    {
        private readonly IGetService<FundInQuery, FundResult> _fundInService;
        private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
        private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
        private readonly IGetService<GameUrlQuery, string> _gameUrlService;
        private readonly IQueryService<GameWagerByBetTimeQuery, GameWagerResult> _gameWagerByBetTimeService;
        private readonly IQueryService<GameWagerByLastUpdateQuery, GameWagerResult> _gameWagerByLastUpdateService;
        private readonly IQueryService<GameWagerByPayTimeQuery, GameWagerResult> _gameWagerByPayTimeService;
        private readonly ILogger<GameInterfaceCards365GrpcService> _logger;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
        private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

        public GameInterfaceCards365GrpcService(
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
            ICommandService<PlayerLogoutCommand> playerLogoutService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameWagerByBetTimeService = gameWagerByBetTimeService ?? throw new ArgumentNullException(nameof(gameWagerByBetTimeService));
            _gameWagerByLastUpdateService = gameWagerByLastUpdateService ?? throw new ArgumentNullException(nameof(gameWagerByLastUpdateService));
            _gameWagerByPayTimeService = gameWagerByPayTimeService ?? throw new ArgumentNullException(nameof(gameWagerByPayTimeService));
            _gameUrlService = gameUrlService ?? throw new ArgumentNullException(nameof(gameUrlService));
            _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
            _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
            _playerLogoutService = playerLogoutService ?? throw new ArgumentNullException(nameof(playerLogoutService));
        }

        public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
        {
            _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

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

            var result = await _fundInService
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

            var result = await _fundOutService
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

        public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                Website: request.Website,
                Cert: request.Cert,
                TransactionId: request.TransactionId);

            var result = await _fundStatusService
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

            var result = await _gameUrlService
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

            var gameWagers = await _gameWagerByBetTimeService
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

            var gameWagers = await _gameWagerByLastUpdateService
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

            var gameWagers = await _gameWagerByPayTimeService
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

            var result = await _playerBalanceService
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

            await _playerCreateService
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

            await _playerLogoutService
               .ExecuteAsync(
                   command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}
