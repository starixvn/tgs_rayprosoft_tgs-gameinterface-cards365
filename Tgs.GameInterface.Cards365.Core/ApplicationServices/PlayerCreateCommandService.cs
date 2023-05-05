using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _ = await _client
        .GetKeyAsync(
            apiUrl: command.ApiUrl,
            website: command.Website,
            cert: command.Cert,
            extension1: command.Extension1,
            user: command.PlayerCode,
            userName: command.PlayerCode,
            currency: command.Currency,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
