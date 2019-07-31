using System;
using System.Threading.Tasks;


namespace Timer.Interfaces
{
	public interface ITimerGrain : Orleans.IGrainWithStringKey
	{
		Task Start(TimeSpan dueTime, TimeSpan period);
		
		Task Stop();

		Task<int> GetCounter();

		Task Count();
	}
}
