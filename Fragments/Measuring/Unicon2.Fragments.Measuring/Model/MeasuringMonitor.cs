using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Model
{
    [DataContract(Namespace = "MeasuringMonitorNS")]
    public class MeasuringMonitor : IMeasuringMonitor
    {
        private IDataProvider _dataProvider;
        private Func<IQuickAccessMemoryApplyingContext> _quickAccessMemoryApplyingContextFunc;
        private List<IMeasuringGroup> _selectedMeasuringGroups;
        private Task _currentCycleLoadingTask;

        public MeasuringMonitor()
        {
            this.MeasuringGroups = new List<IMeasuringGroup>();
        }


        public void StopLoadingCycle()
        {
            this.IsLoadingCycleInProcess = false;
        }

        public bool IsLoadingCycleInProcess { get; private set; }

        public string StrongName => MeasuringKeys.MEASURING_MONITOR;

        [DataMember]
        public IFragmentSettings FragmentSettings { get; set; }

        [DataMember]
        public List<IMeasuringGroup> MeasuringGroups { get; set; }

        public void SetSelectedGroups(List<IMeasuringGroup> measuringGroups)
        {
            this._selectedMeasuringGroups = measuringGroups;
        }

    }
}
