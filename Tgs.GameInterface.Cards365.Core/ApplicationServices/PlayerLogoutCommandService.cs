using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class PlayerLogoutCommandService : ICommandService<PlayerLogoutCommand>
{
    private readonly IGameApiClient _client;

    public PlayerLogoutCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => _client.LogoutAsync(
            apiUrl: command.ApiUrl,
            website: command.Website,
            cert: command.Cert,
            extension1: command.Extension1,
            user: command.PlayerCode,
            cancellationToken: cancellationToken);
}
