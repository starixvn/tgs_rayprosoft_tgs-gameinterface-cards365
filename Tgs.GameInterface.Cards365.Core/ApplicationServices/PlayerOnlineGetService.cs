using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

public class PlayerOnlineGetService(
	IGameApiClient client) : IGetService<PlayerOnlineQuery, bool>
{
	public async ValueTask<bool> GetAsync(PlayerOnlineQuery query, CancellationToken cancellationToken = default)
		=> await client.GetUserStatusAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			extension1: query.Extension1,
			user: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
}
