using Collatz.Core;
using Ooui;
using System.Numerics;

namespace Collatz.OouiClient
{
	class Program
	{
		static void Main()
		{
			var title = new Heading("Collatz.Ooui");
			var input = new Input();
			var button = new Button("Calculate");
			var result = new Div();

			button.Click += (s, e) => 
			{
				if (BigInteger.TryParse(input.Value, out var start))
				{
					var sequence = new CollatzSequence(start);
					//result.Text = string.Join(", ", sequence.Sequence);
					result.Text = start.ToString();
				}
				else
				{
					result.Text = $"{input.Value} is not a valid BigInteger.";
				}
			};

			var container = new Div(title, input, button, result);

			UI.Publish("/", container);
		}
	}
}
