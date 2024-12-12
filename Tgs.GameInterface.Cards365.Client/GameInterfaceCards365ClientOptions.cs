namespace Tgs.GameInterface.Cards365.Client;

public class GameInterfaceCards365ClientOptions
{
	public Uri BaseUrl { get; set; } = default!;
	public TimeSpan GrpcConnectTimeout { get; set; } = TimeSpan.FromSeconds(5);
}
