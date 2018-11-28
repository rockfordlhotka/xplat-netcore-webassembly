using Autofac;
using Collatz.Api.Logging;

namespace Collatz.Api
{
	public sealed class ApiModule
		: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);
			builder.RegisterType<SerilogLogging>().As<ILogger>().SingleInstance();
		}
	}
}
