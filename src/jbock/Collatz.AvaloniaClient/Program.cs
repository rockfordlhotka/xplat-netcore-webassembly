using Avalonia;
using Avalonia.Logging.Serilog;

namespace Collatz.AvaloniaClient
{
	class Program
	{
		static void Main() => Program.BuildAvaloniaApp().Start<MainWindow>();

		public static AppBuilder BuildAvaloniaApp() => 
			AppBuilder.Configure<App>()
				.UsePlatformDetect()
				.LogToDebug();
	}
}
