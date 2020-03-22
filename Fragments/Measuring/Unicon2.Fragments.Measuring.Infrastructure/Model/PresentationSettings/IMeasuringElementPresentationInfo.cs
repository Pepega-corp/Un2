using System;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
	public interface IMeasuringElementPresentationInfo: IMeasuringPresentationElement
	{
		Guid RelatedMeasuringElementId { get; set; }

	}
}