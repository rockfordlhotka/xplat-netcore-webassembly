using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Collatz.Core;
using System.Numerics;

namespace Collatz.AvaloniaClient
{
	public class MainWindow : Window
	{
		public MainWindow() => this.InitializeComponent();

		private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

		public void OnCalculateSequence(object sender, RoutedEventArgs args)
		{
			var value = this.FindControl<TextBox>("NumberOfItems").Text;

			if(BigInteger.TryParse(value, out var start))
			{
				var sequence = new CollatzSequence(start);
				this.FindControl<ListBox>("Results").Items = sequence.Sequence;
			}
		}
	}
}
