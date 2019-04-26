using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Collatz.Server
{
	public class Program
	{
		public static void Main(string[] args) => 
			Program.BuildWebHost(args).Run();

		public static IWebHost BuildWebHost(string[] args) =>
			 WebHost.CreateDefaultBuilder(args)
				  .UseConfiguration(new ConfigurationBuilder()
						.AddCommandLine(args)
						.Build())
				  .UseStartup<Startup>()
				  .Build();
	}
}