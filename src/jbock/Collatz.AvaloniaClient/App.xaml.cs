using Avalonia;
using Avalonia.Markup.Xaml;

namespace Collatz.AvaloniaClient
{
	public class App : Application
	{
		public override void Initialize() => AvaloniaXamlLoader.Load(this);
	}
}
