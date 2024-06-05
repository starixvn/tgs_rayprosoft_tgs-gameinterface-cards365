using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> _ = await client.GetKeyAsync(
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
