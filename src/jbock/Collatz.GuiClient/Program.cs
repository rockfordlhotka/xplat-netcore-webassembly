using Collatz.Core;
using System.Collections.Immutable;
using System.Numerics;
using Terminal.Gui;

namespace Collatz.GuiClient
{
	class Program
	{
		static void Main()
		{
			Application.Init();
			var top = Application.Top;

			var window = new Window(new Rect(0, 1, top.Frame.Width, top.Frame.Height - 1), "Collatz gui.cs-Based Application");
			top.Add(window);

			var menu = new MenuBar(new MenuBarItem[] 
			{
				new MenuBarItem("_Application", new MenuItem [] 
				{
					 new MenuItem("_Quit", "", () => top.Running = false)
				})
			});
			top.Add(menu);

			var results = new ListView(new Rect(3, 6, top.Frame.Width - 6, 10), ImmutableArray<BigInteger>.Empty);
			var value = new TextField(14, 2, 40, "");
			var button = new Button(3, 4, "Generate")
			{
				Clicked = () => 
				{
					if(BigInteger.TryParse(value.Text.ToString(), out var start))
					{
						var sequence = new CollatzSequence(start);
						results.SetSource(sequence.Sequence);
					}
				}
			};

			window.Add(
				new Label(3, 2, "Start: "),
				value, button, results,  
				new Label(3, 18, "Press ESC and 9 to activate the menubar"));

			Application.Run();
		}
	}
}