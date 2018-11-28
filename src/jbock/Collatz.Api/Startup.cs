using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = Startup.ApiName,
					Description = "Contains a GET method to calculate a Collatz sequence.",
					TermsOfService = "None",
					Contact = new Contact() { Name = "Jason Bock", Email = "jason@jasonbock.net", Url = "www.jasonbock.net" }
				});
			});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", Startup.ApiName);
			});
		}

		public IConfiguration Configuration { get; }
	}
}
