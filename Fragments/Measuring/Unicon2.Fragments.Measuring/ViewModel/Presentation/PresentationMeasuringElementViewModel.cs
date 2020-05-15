using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.ViewModel.Presentation
{
	public class PresentationMeasuringElementViewModel
	{
		public PresentationMeasuringElementViewModel(IPositioningInfo positioningInfo, object templatedObjectOnCanvas)
		{
			PositioningInfo = positioningInfo;
			TemplatedObjectOnCanvas = templatedObjectOnCanvas;
		}

		public IPositioningInfo PositioningInfo { get; }
		public object TemplatedObjectOnCanvas { get; }
	}
}