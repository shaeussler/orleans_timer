using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Timer.Grains;
using Timer.Interfaces;

namespace SiloHost
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			try
			{
				var host = await StartSilo();

				Console.WriteLine("Press Enter to terminate...");

				var client = host.Services.GetRequiredService<IClusterClient>();

				var timerGrain = client.GetGrain<ITimerGrain>("MSFT");

				Console.WriteLine("Start Orleans timer.");

				//await timerGrain.Start(TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));
				await timerGrain.Start(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100));

				Console.WriteLine("Wait for 30s.");

				await Task.Delay(30 * 1000);

				Console.WriteLine("Stop Orleans timer.");

				await timerGrain.Stop();

				var counter = await timerGrain.GetCounter();

				Console.WriteLine("Counter is {0}", counter);

				Console.ReadLine();

				await host.StopAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private static async Task<ISiloHost> StartSilo()
		{
			// define the cluster configuration
			var builder = new SiloHostBuilder()
				 .UseLocalhostClustering()
				 .Configure<ClusterOptions>(options =>
				 {
					 options.ClusterId = "dev";
					 options.ServiceId = "TimerSampleApp";
				 })
				.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TimerGrain).Assembly).WithReferences())
				 .EnableDirectClient();

			var host = builder.Build();
			await host.StartAsync();
			return host;
		}
	}
}
