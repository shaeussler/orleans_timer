using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Orleans;
using Timer.Interfaces;

namespace Timer.Grains
{
	public class TimerGrain : Grain, ITimerGrain
	{
		private int _counter;
		private IDisposable _timer;
		private Stopwatch _stopwatch;
		private Stopwatch _timerStopwatch;

		public override async Task OnActivateAsync()
		{
			await base.OnActivateAsync();
		}

		private Task CountCallback(object arg)
		{
			_counter++;

			Console.WriteLine($"timerStopwatch.ElapsedMilliseconds: {_timerStopwatch.ElapsedMilliseconds} ms, counter: {_counter}");

			//await this.AsReference<ITimerGrain>().Count();

			return Task.CompletedTask;
		}

		public Task Start(TimeSpan dueTime, TimeSpan period)
		{
			_stopwatch = new Stopwatch();
			_timerStopwatch = new Stopwatch();

			_timerStopwatch.Start();
			_stopwatch.Start();

			_timer = RegisterTimer(
					 CountCallback,
					 null,
					 dueTime,
					 period);

			return Task.CompletedTask;
		}

		public Task Stop()
		{
			_stopwatch.Stop();
			Console.WriteLine($"stopwatch.ElapsedMilliseconds: {_stopwatch.ElapsedMilliseconds} ms");

			_timer.Dispose();

			return Task.CompletedTask;
		}

		public Task<int> GetCounter()
		{
			return Task.FromResult(_counter);
		}

		public Task Count()
		{
			_counter++;

			Console.WriteLine($"timerStopwatch.ElapsedMilliseconds: {_timerStopwatch.ElapsedMilliseconds} ms, counter: {_counter}");

			return Task.CompletedTask;
		}
	}
}
