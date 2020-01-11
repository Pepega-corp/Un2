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

        public MeasuringMonitor(Func<IQuickAccessMemoryApplyingContext> quickAccessMemoryApplyingContextFunc)
        {
            this._quickAccessMemoryApplyingContextFunc = quickAccessMemoryApplyingContextFunc;
            this.MeasuringGroups = new List<IMeasuringGroup>();
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;


            foreach (IMeasuringGroup measuringGroup in this.MeasuringGroups)
            {
                measuringGroup.SetDataProvider(dataProvider);
            }

            if (this.FragmentSettings != null)
            {
                foreach (IFragmentSetting configurationSetting in this.FragmentSettings.FragmentSettings)
                {
                    if (configurationSetting is IDataProviderContaining)
                    {
                        (configurationSetting as IDataProviderContaining).SetDataProvider(dataProvider);
                    }
                }
            }
        }

        public async Task Load()
        {
            if (this._dataProvider == null) return;
            await this.LoadSettingsAsync();
            for (int i = 0; i < this._selectedMeasuringGroups.Count; i++)
            {
                await this._selectedMeasuringGroups[i].Load();
            }

            this.SetDataProvider(this._dataProvider);
        }



        private async Task LoadSettingsAsync()
        {
            IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext = this._quickAccessMemoryApplyingContextFunc();
            quickAccessMemoryApplyingContext.QueryNameKey = MeasuringKeys.READ_MEASURING_SIGNALS_QUERY;
            foreach (IMeasuringGroup measuringGroup in this._selectedMeasuringGroups)
            {
                quickAccessMemoryApplyingContext.DataProviderContainingObjectList.Add(measuringGroup);
            }
            quickAccessMemoryApplyingContext.QuickAccessMode = QuickAccessModeEnum.Initialize;
            Task<bool> applySettingByKeyTask = this.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);
            if (applySettingByKeyTask != null)
                await applySettingByKeyTask;
        }

        public async void StartLoadingCycle()
        {
            if (this._dataProvider == null) return;
            this.IsLoadingCycleInProcess = true;
            if ((this._currentCycleLoadingTask == null) || (this._currentCycleLoadingTask.IsCompleted))
            {
                this._currentCycleLoadingTask = this.LoadingCycle();
                await this._currentCycleLoadingTask;
            }
        }

        private async Task LoadingCycle()
        {
            while (_dataProvider.LastQuerySucceed != false)
            {
                if (this.IsLoadingCycleInProcess)
                {
                    await this.Load();
                }
                else
                {
                    return;
                }
            }
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

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;
            foreach (IMeasuringGroup measuringGroup in this.MeasuringGroups)
            {
                measuringGroup.InitializeFromContainer(container);
            }
            this._quickAccessMemoryApplyingContextFunc = container.Resolve<Func<IQuickAccessMemoryApplyingContext>>();
            (this.FragmentSettings as IInitializableFromContainer)?.InitializeFromContainer(container);

            this.IsInitialized = true;
        }
    }
}
