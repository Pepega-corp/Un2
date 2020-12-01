using System.Threading.Tasks;

namespace Unicon2.Fragments.Measuring.Subscriptions
{
	public interface IMeasuringMemorySubscription
	{
		string GroupName { get; }
		Task Execute();
	}

}
