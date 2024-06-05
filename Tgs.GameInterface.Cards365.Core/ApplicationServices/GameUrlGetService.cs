using System.Web;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Cards365.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, string>
{
	public async ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var key = await client.GetKeyAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			extension1: query.Extension1,
			user: query.PlayerCode,
			userName: query.PlayerCode,
			currency: query.Currency,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var url = new Uri(query.ApiUrl, $"/api/{query.Website}/loginV2");
		var builder = new UriBuilder(url);
		var nameValueCollection = HttpUtility.ParseQueryString(builder.Query);
		nameValueCollection["user"] = query.PlayerCode;
		nameValueCollection["key"] = key;
		nameValueCollection["extension1"] = query.Extension1;
		nameValueCollection["userName"] = query.PlayerCode;
		nameValueCollection["language"] = query.Language;
		nameValueCollection["fullscreen"] = "0";
		nameValueCollection["jackpot"] = "0";
		nameValueCollection["blockfree"] = "1";
		builder.Query = nameValueCollection.ToString();

		return builder.ToString();
	}
}
