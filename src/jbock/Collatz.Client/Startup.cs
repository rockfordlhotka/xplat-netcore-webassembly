using Collatz.Client.Mvvm;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Collatz.Client
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddTelerikBlazor();
			services.AddTransient<SequenceViewModel>();
		}

		public void Configure(IComponentsApplicationBuilder app) => 
			app.AddComponent<App>("app");
	}
}