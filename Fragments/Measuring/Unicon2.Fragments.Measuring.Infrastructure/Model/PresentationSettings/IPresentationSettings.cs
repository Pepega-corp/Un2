using System.Collections.Generic;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings
{
	public interface IPresentationSettings
	{
		List<IMeasuringPresentationGroup> GroupsOfElements { get; set; }
		List<IMeasuringElementPresentationInfo> Elements { get; set; }
	}
}