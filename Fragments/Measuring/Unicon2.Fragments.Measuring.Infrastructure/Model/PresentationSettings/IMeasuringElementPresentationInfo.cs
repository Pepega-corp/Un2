using System;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings
{
	public interface IMeasuringElementPresentationInfo : IMeasuringPresentationElement
	{
		Guid RelatedMeasuringElementId { get; set; }

	}
}