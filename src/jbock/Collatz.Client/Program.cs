using Microsoft.AspNetCore.Blazor.Hosting;

namespace Collatz.Client
{
	public class Program
	{
		public static void Main() => 
			Program.CreateHostBuilder().Build().Run();

		public static IWebAssemblyHostBuilder CreateHostBuilder() =>
			 BlazorWebAssemblyHost.CreateDefaultBuilder()
				  .UseBlazorStartup<Startup>();
	}
}