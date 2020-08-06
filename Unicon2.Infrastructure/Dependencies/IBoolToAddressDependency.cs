using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Dependencies
{
	public interface IBoolToAddressDependency : IDependency
	{
		string RelatedResourceName { get; set; }
		ushort ResultingAddressIfTrue { get; set; }
		ushort ResultingAddressIfFalse { get; set; }
		IUshortsFormatter FormatterIfTrue { get; set; }
		IUshortsFormatter FormatterIfFalse { get; set; }

	}
}
