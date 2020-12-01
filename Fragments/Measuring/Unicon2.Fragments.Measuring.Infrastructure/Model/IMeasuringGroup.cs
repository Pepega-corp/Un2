using System.Collections.Generic;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
    public interface IMeasuringGroup : INameable
    {
        List<IMeasuringElement> MeasuringElements { get; set; }
        IPresentationSettings PresentationSettings { get; set; }
    }
}