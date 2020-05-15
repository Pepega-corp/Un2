namespace Unicon2.Infrastructure.Dependencies
{
	public interface IBoolToAddressDependency:IDependency
	{
		string RelatedResourceName { get; set; }
		ushort ResultingAddressIfTrue { get; set; }
		ushort ResultingAddressIfFalse { get; set; }

	}
}
