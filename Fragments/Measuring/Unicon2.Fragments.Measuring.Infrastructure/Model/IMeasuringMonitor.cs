using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
    public interface IMeasuringMonitor : IDeviceFragment
    {
        List<IMeasuringGroup> MeasuringGroups { get; set; }
    }
}