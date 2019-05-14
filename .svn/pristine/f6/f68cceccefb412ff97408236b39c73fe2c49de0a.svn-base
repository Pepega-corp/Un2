using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Progress;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.Model
{
    public interface IOscilloscopeModel:IDeviceFragment, IParentDeviceNameRequirable
    {
        IUniconJournal OscilloscopeJournal { get; set; }
        IOscillogramLoadingParameters OscillogramLoadingParameters { get; set; }
        ICountingTemplate CountingTemplate { get; set; }
        Task LoadOscillogramsByNumber(List<int> numbersOfOscillograms, IProgress<ITaskProgressReport> progress, CancellationToken cancellationToken);
        bool TryGetOscillogram(int index,out string oscillogramPath);

    }


}
