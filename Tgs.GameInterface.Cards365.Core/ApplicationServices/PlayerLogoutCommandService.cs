using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class PlayerLogoutCommandService(IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => client.LogoutAsync(
            apiUrl: command.ApiUrl,
            website: command.Website,
            cert: command.Cert,
            extension1: command.Extension1,
            user: command.PlayerCode,
            cancellationToken: cancellationToken);
}
