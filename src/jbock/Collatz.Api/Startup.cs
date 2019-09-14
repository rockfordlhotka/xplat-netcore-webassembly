using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System;

namespace Collatz.Api
{
	public class Startup
	{
		private const string ApiName = "Collatz Conjecture API";

		public Startup(IConfiguration configuration) =>
			this.Configuration = configuration;

		public void ConfigureContainer(ContainerBuilder builder) =>
			builder.RegisterModule(new ApiModule());

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = Startup.ApiName,
					Description = "Contains a GET method to calculate a Collatz sequence.",
					Contact = new OpenApiContact() 
					{ 
						Name = "Jason Bock", 
						Email = "jason@jasonbock.net", 
						Url = new Uri("http://www.jasonbock.net") 
					}
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", Startup.ApiName);
			});
		}

		public IConfiguration Configuration { get; }
	}
}