using Collatz.Core;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Collatz.Client.Mvvm
{
	public sealed class SequenceViewModel
	{
		private readonly IJSRuntime runtime;

		public SequenceViewModel(IJSRuntime runtime) =>
			this.runtime = runtime;

		public async Task GetGeolocationAsync() =>
			await this.runtime.InvokeAsync<object>(
				"getGeolocation", new DotNetObjectRef(this));

		[JSInvokable]
		public void Change(double latitude, double longitude, double accuracy)
		{
			(this.Latitude, this.Longitude, this.Accuracy) = 
				(latitude, longitude, accuracy);
			this.BingMainUrl = $"https://www.bing.com/maps/embed?h=400&w=500&cp={latitude}~{longitude}&lvl=11&typ=d&sty=r&src=SHELL&FORM=MBEDV8";
			this.BingLargeMapUrl = $"https://www.bing.com/maps?cp={latitude}~{longitude}&amp;sty=r&amp;lvl=11&amp;FORM=MBEDLD";
			this.BingDirectionsUrl = $"https://www.bing.com/maps/directions?cp={latitude}~-{longitude}&amp;sty=r&amp;lvl=11&amp;rtp=~pos.{latitude}_{longitude}____&amp;FORM=MBEDLD";
			this.Changed?.Invoke(this, EventArgs.Empty);
		}

		public async void CreateSequence()
		{
			if (BigInteger.TryParse(this.Value, out var value))
			{
				try
				{
					var sequence = new CollatzSequence(value);
					this.CurrentSequence = string.Join(", ", sequence.Sequence);
					this.Sequence = sequence.Sequence.Select(_ => (object)(int)_).ToList();
					this.Labels = Enumerable.Range(1, sequence.Sequence.Length).Select(_ => _.ToString()).ToArray();
					this.Changed?.Invoke(this, EventArgs.Empty);
					await this.runtime.InvokeAsync<object>(
						"updateChart", this.CollatzChartId,
						sequence.Sequence.Select(_ => (int)_).ToArray(), this.Labels);
				}
				catch (ArgumentException)
				{
					this.CurrentSequence = $"The value, {value}, is incorrect.";
					this.Sequence = default;
					this.Labels = Array.Empty<string>();
					await this.runtime.InvokeAsync<object>(
						"updateChart", this.CollatzChartId, 
						Array.Empty<int>(), Array.Empty<string>());
					this.Changed?.Invoke(this, EventArgs.Empty);
				}
			}
			else
			{
				this.CurrentSequence = $"{this.Value} is not a valid integer.";
				this.Sequence = default;
				this.Labels = Array.Empty<string>();
				await this.runtime.InvokeAsync<object>(
					"updateChart", this.CollatzChartId, 
					Array.Empty<int>(), Array.Empty<string>());
				this.Changed?.Invoke(this, EventArgs.Empty);
			}
		}

		public double Accuracy { get; private set; }
		public string BingMainUrl { get; private set; }
		public string BingLargeMapUrl { get; private set; }
		public string BingDirectionsUrl { get; private set; }
		public string CollatzChartId => "collatzChart";
		public string CurrentSequence { get; private set; }
		public string[] Labels { get; private set; }
		public double Latitude { get; private set; }
		public double Longitude { get; private set; }
		public List<object> Sequence { get; private set; }
		public string Value { get; set; }

		public event EventHandler Changed;
	}
}