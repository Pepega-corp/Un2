namespace Unicon2.Infrastructure.DeviceInterfaces.SharedResources
{
	public interface IResourceContainer
	{
		string ResourceName { get; set; }
		object Resource { get; set; }
	}
}