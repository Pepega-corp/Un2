using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	public class DateTimeMeasuringElement : MeasuringElementBase, IDateTimeMeasuringElement
	{
		public override string StrongName => MeasuringKeys.DATE_TIME_ELEMENT;
	}
}