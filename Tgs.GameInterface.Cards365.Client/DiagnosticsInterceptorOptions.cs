using System.Diagnostics;

namespace Tgs.GameInterface.Cards365.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.Cards365.Client.DiagnosticsInterceptor");
}
