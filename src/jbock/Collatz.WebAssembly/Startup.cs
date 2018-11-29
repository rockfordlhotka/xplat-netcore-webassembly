using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace Collatz.WebAssembly
{
	public class Startup
	{
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			var provider = new FileExtensionContentTypeProvider();
			provider.Mappings[".wasm"] = "application/octet-stream";
			app.UseStaticFiles(new StaticFileOptions()
			{
				ContentTypeProvider = provider
			});
		}
	}
}