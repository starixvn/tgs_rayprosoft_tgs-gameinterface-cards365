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
        private readonly IQueryService<GameRecordByBetTimeQuery, Core.Models.GameRecordInfo> _gameRecordByBetTimeService;
        private readonly IQueryService<GameRecordByLastUpdateQuery, Core.Models.GameRecordInfo> _gameRecordByLastUpdateService;
        private readonly IQueryService<GameRecordByPayTimeQuery, Core.Models.GameRecordInfo> _gameRecordByPayTimeService;
        private readonly IGetService<GameUrlQuery, string> _gameUrlService;
        private readonly ILogger<GameInterfaceCards365GrpcService> _logger;
        private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
        private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
        private readonly ICommandService<PlayerLogoutCommand> _playerLogoutService;

        public GameInterfaceCards365GrpcService(
            ILogger<GameInterfaceCards365GrpcService> logger,
            IGetService<FundInQuery, FundResult> fundInService,
            IGetService<FundOutQuery, FundResult> fundOutService,
            IGetService<FundStatusQuery, bool> fundStatusService,
            IQueryService<GameRecordByBetTimeQuery, Core.Models.GameRecordInfo> gameRecordByBetTimeService,
            IQueryService<GameRecordByLastUpdateQuery, Core.Models.GameRecordInfo> gameRecordByLastUpdateService,
            IQueryService<GameRecordByPayTimeQuery, Core.Models.GameRecordInfo> gameRecordByPayTimeService,
            IGetService<GameUrlQuery, string> gameUrlService,
            IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
            ICommandService<PlayerCreateCommand> playerCreateService,
            ICommandService<PlayerLogoutCommand> playerLogoutService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
            _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
            _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
            _gameRecordByBetTimeService = gameRecordByBetTimeService ?? throw new ArgumentNullException(nameof(gameRecordByBetTimeService));
            _gameRecordByLastUpdateService = gameRecordByLastUpdateService ?? throw new ArgumentNullException(nameof(gameRecordByLastUpdateService));
            _gameRecordByPayTimeService = gameRecordByPayTimeService ?? throw new ArgumentNullException(nameof(gameRecordByPayTimeService));
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
                TsCode = result.TsCode
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
                TsCode = result.TsCode
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

        public override async Task<GameRecordReply> GameRecordByBetTime(GameRecordByBetTimeRequest request, ServerCallContext context)
        {
            var query = new GameRecordByBetTimeQuery(
                ApiUrl: new(request.ApiUrl),
                Website: request.Website,
                Cert: request.Cert,
                Extension1: request.Extension1,
                StartTime: request.StartTime.ToDateTimeOffset(),
                EndTime: request.EndTime.ToDateTimeOffset());

            var gameRecords = await _gameRecordByBetTimeService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<Core.Models.GameRecordInfo>();

            var reply = new GameRecordReply();

            if (gameRecords.Any())
            {
                foreach (var gameRecord in gameRecords)
                {
                    reply.Items.Add(new GameRecordInfo
                    {
                        ExternalRoundId = gameRecord.ExternalRoundId,
                        ExternalGameId = gameRecord.ExternalGameId,
                        PlayerCode = gameRecord.PlayerCode,
                        IsClosed = gameRecord.IsClosed,
                        IsVoid = gameRecord.IsVoid,
                        UpdateTime = gameRecord.UpdateTime.ToTimestamp(),
                        ExternalCreatedOn = gameRecord.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = gameRecord.ExternalClosedOn.ToTimestamp(),
                        Bet = gameRecord.Bet,
                        ValidBet = gameRecord.Validbet,
                        Settle = gameRecord.Settle,
                        Winloss = gameRecord.Winloss,
                        Commission = gameRecord.Commission,
                        AdditionalData = gameRecord.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameRecordReply> GameRecordByLastUpdate(GameRecordByLastUpdateRequest request, ServerCallContext context)
        {
            var query = new GameRecordByLastUpdateQuery(
                ApiUrl: new(request.ApiUrl),
                Website: request.Website,
                Cert: request.Cert,
                Extension1: request.Extension1,
                Lastupdatedate: request.Lastupdatedate);

            var gameRecords = await _gameRecordByLastUpdateService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<Core.Models.GameRecordInfo>();

            var reply = new GameRecordReply();

            if (gameRecords.Any())
            {
                foreach (var gameRecord in gameRecords)
                {
                    reply.Items.Add(new GameRecordInfo
                    {
                        ExternalRoundId = gameRecord.ExternalRoundId,
                        ExternalGameId = gameRecord.ExternalGameId,
                        PlayerCode = gameRecord.PlayerCode,
                        IsClosed = gameRecord.IsClosed,
                        IsVoid = gameRecord.IsVoid,
                        UpdateTime = gameRecord.UpdateTime.ToTimestamp(),
                        ExternalCreatedOn = gameRecord.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = gameRecord.ExternalClosedOn.ToTimestamp(),
                        Bet = gameRecord.Bet,
                        ValidBet = gameRecord.Validbet,
                        Settle = gameRecord.Settle,
                        Winloss = gameRecord.Winloss,
                        Commission = gameRecord.Commission,
                        AdditionalData = gameRecord.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameRecordReply> GameRecordByPayTime(GameRecordByPayTimeRequest request, ServerCallContext context)
        {
            var query = new GameRecordByPayTimeQuery(
                ApiUrl: new(request.ApiUrl),
                Website: request.Website,
                Cert: request.Cert,
                Extension1: request.Extension1,
                StartTime: request.StartTime.ToDateTimeOffset(),
                EndTime: request.EndTime.ToDateTimeOffset());

            var gameRecords = await _gameRecordByPayTimeService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<Core.Models.GameRecordInfo>();

            var reply = new GameRecordReply();

            if (gameRecords.Any())
            {
                foreach (var gameRecord in gameRecords)
                {
                    reply.Items.Add(new GameRecordInfo
                    {
                        ExternalRoundId = gameRecord.ExternalRoundId,
                        ExternalGameId = gameRecord.ExternalGameId,
                        PlayerCode = gameRecord.PlayerCode,
                        IsClosed = gameRecord.IsClosed,
                        IsVoid = gameRecord.IsVoid,
                        UpdateTime = gameRecord.UpdateTime.ToTimestamp(),
                        ExternalCreatedOn = gameRecord.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = gameRecord.ExternalClosedOn.ToTimestamp(),
                        Bet = gameRecord.Bet,
                        ValidBet = gameRecord.Validbet,
                        Settle = gameRecord.Settle,
                        Winloss = gameRecord.Winloss,
                        Commission = gameRecord.Commission,
                        AdditionalData = gameRecord.AdditionalData
                    });
                }
            }

            return reply;
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
