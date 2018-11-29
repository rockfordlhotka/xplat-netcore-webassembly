using AspNetMonsters.Blazor.Geolocation;
using Collatz.BlazorClient.App.Services;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Collatz.BlazorClient.App
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			// Since Blazor is running on the server, we can use an application service
			// to read the forecast data.
			services.AddSingleton<WeatherForecastService>();
			services.AddSingleton<LocationService>();
		}

		public void Configure(IBlazorApplicationBuilder app) =>
			app.AddComponent<App>("app");
	}
}
