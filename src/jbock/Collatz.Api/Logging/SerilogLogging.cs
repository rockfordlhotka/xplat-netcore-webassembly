using Serilog;
using Serilog.Core;

namespace Collatz.Api.Logging
{
	internal sealed class SerilogLogging
		: ILogger
	{
		private readonly Logger logger;

		public SerilogLogging() => 
			this.logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

		public void Log(string message) => 
			this.logger.Information(message);
	}
}
