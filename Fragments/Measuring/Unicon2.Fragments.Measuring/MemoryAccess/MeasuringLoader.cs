using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Subscriptions;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.MemoryAccess
{
	public class MeasuringLoader
	{
		private DeviceContext _deviceContext;
		private MeasuringSubscriptionSet _measuringSubscriptionSet;
		private readonly IMeasuringMonitor _measuringMonitor;
	    private readonly RelayCommand _dependentCommand;
	    private readonly FragmentOptionToggleCommandViewModel _readCycleCommand;
	    private string _groupName;
		private bool _isQueriesStarted = false;
	    private bool _isLoadInProgress;

	    public MeasuringLoader(DeviceContext deviceContext, MeasuringSubscriptionSet measuringSubscriptionSet,IMeasuringMonitor measuringMonitor,RelayCommand dependentCommand,FragmentOptionToggleCommandViewModel readCycleCommand)
		{
			_deviceContext = deviceContext;
			_measuringSubscriptionSet = measuringSubscriptionSet;
			_measuringMonitor = measuringMonitor;
		    this._dependentCommand = dependentCommand;
		    _readCycleCommand = readCycleCommand;
		}

	    public async void StartLoading()
	    {
		    if (this._isQueriesStarted) return;
		    _isQueriesStarted = true;
		    if (IsLoadInProgress) return;
		    try
		    {
			    await this.Load();
		    }
		    catch
		    {
			    ErrorOccured = true;
		    }
	    }

	    public void StopLoading()
		{
			_isQueriesStarted = false;
		}

		public void SetCurrentGroup(string groupName=null)
		{
			_groupName = groupName;
		}

		public bool IsLoadInProgress
		{
			get { return this._isLoadInProgress; }
			private set
			{
				this._isLoadInProgress = value;
				this._dependentCommand?.RaiseCanExecuteChanged();
				if (!value)
					_readCycleCommand.IsChecked = false;
			}
		}

		public bool ErrorOccured { get; set; }

		public async void ExecuteLoad()
		{
			if (this._isQueriesStarted) return;
			await Load();
		}

		private async Task Load()
		{
			try
			{
				this.IsLoadInProgress = true;
				while (true)
				{
					if (!_deviceContext.DataProviderContainer.DataProvider.IsSuccess)
					{
						this.IsLoadInProgress = false;
						return;
					}

					await LoadMemory();
					foreach (var discreteSubscription in _measuringSubscriptionSet.DiscreteSubscriptions)
					{
						if (_groupName != null && discreteSubscription.GroupName != _groupName)
						{
							continue;
						}

						await discreteSubscription.Execute();
					}

					foreach (var analogSubscription in _measuringSubscriptionSet.AnalogSubscriptions)
					{
						if (_groupName != null && analogSubscription.GroupName != _groupName)
						{
							continue;
						}

						await analogSubscription.Execute();
					}

					foreach (var dateTimeSubscription in _measuringSubscriptionSet.DateTimeSubscriptions)
					{
						if (_groupName != null && dateTimeSubscription.GroupName != _groupName)
						{
							continue;
						}

						await dateTimeSubscription.Execute();
					}

					if (!this._isQueriesStarted)
					{
						this.IsLoadInProgress = false;
						return;
					}
				}
			}
			catch
			{
				ErrorOccured = true;
			}
		}

		private async Task LoadMemory()
		{
			List<ushort> addressesToLoadFun3=new List<ushort>();
		    List<ushort> addressesToLoadFun1 = new List<ushort>();

            foreach (var discreteSubscription in _measuringSubscriptionSet.DiscreteSubscriptions)
			{
				if (_groupName != null && discreteSubscription.GroupName != _groupName)
				{
					continue;
				}
				if (discreteSubscription.DiscretMeasuringElement.AddressOfBit.NumberOfFunction == 3)
				{
					addressesToLoadFun3.Add(discreteSubscription.DiscretMeasuringElement.Address);
				}
			    if (discreteSubscription.DiscretMeasuringElement.AddressOfBit.NumberOfFunction == 1)
			    {
			        addressesToLoadFun1.Add(discreteSubscription.DiscretMeasuringElement.Address);
			    }

            }
		    foreach (var discreteSubscription in _measuringSubscriptionSet.AnalogSubscriptions)
		    {
		        if (_groupName != null && discreteSubscription.GroupName != _groupName)
		        {
		            continue;
		        }
		        addressesToLoadFun3.Add(discreteSubscription.AnalogMeasuringElement.Address);
		    }
		    ClearExistingAddressesInMemory(_deviceContext.DeviceMemory.DeviceMemoryValues, _deviceContext.DeviceMemory.DeviceBitMemoryValues, addressesToLoadFun3,addressesToLoadFun1);
			await LoadSettings(addressesToLoadFun3);
			await LoadAddresses(_deviceContext.DeviceMemory.DeviceMemoryValues,this._deviceContext.DeviceMemory.DeviceBitMemoryValues,addressesToLoadFun3,addressesToLoadFun1);
		}

	    private async Task LoadAddresses(Dictionary<ushort, ushort> memoryDictionaryUshort, Dictionary<ushort, bool> memoryDictionaryBit,
            List<ushort> addressesToLoadFun3, List<ushort> addressesToLoadFun1)
	    {
	        if ((!(addressesToLoadFun3.Any() && addressesToLoadFun1.Any()))||!_deviceContext.DataProviderContainer.DataProvider.IsSuccess)
	        {
	            await Task.Delay(100);
	            return;
	        }

	     
	        foreach (var addressUshort in addressesToLoadFun3)
	        {
	            if (!memoryDictionaryUshort.ContainsKey(addressUshort))
	            {
	                var res = await _deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(
	                    addressUshort, 1, "Read measuring");
	                if (res.IsSuccessful)
	                {
	                    memoryDictionaryUshort.Add(addressUshort, res.Result.First());
	                }
	            }
	        }

	        if (addressesToLoadFun1.Any())
	        {
	            foreach (var addressUshort in addressesToLoadFun1)
	            {
	                if (!memoryDictionaryBit.ContainsKey(addressUshort))
	                {
	                    var res = await _deviceContext.DataProviderContainer.DataProvider.Item.ReadCoilStatusAsync(
	                        addressUshort, "Read measuring");
	                    if (res.IsSuccessful)
	                    {
	                        memoryDictionaryBit.Add(addressUshort, res.Result);
	                    }
	                }
	            }

	        }
	    }

	    private void ClearExistingAddressesInMemory(Dictionary<ushort, ushort> memoryDictionary, Dictionary<ushort, bool> memoryBitDictionary, List<ushort> addressesToLoadFun3, List<ushort> addressesToLoadFun1)
		{
			foreach (var addressUshort in addressesToLoadFun3)
			{
				if (memoryDictionary.ContainsKey(addressUshort))
				{
					memoryDictionary.Remove(addressUshort);
				}
		    }
		    foreach (var addressUshort in addressesToLoadFun1)
		    {
		        if (memoryBitDictionary.ContainsKey(addressUshort))
		        {
		            memoryBitDictionary.Remove(addressUshort);
		        }
		    }
        }

		private async Task LoadSettings(List<ushort> addressesToLoadFun3)
		{
			IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext =
				StaticContainer.Container.Resolve<IQuickAccessMemoryApplyingContext>();


			quickAccessMemoryApplyingContext.OnFillAddressRange =
				(range) =>
				{
					ushort rangeFrom = (ushort)range.RangeFrom;
					ushort rangeTo = (ushort)range.RangeTo;
					
						return ReadSettingsAddresses(rangeFrom, rangeTo,addressesToLoadFun3);
					
				};


			Task applySettingByKey = _measuringMonitor.FragmentSettings?.ApplySettingByKey(
				ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
				quickAccessMemoryApplyingContext);

			if (applySettingByKey != null)
				await applySettingByKey;

		}

		private async Task ReadSettingsAddresses(ushort rangeFrom, ushort rangeTo, List<ushort> addressesToLoadFun3)
		{
			if (addressesToLoadFun3.Any(arg => rangeFrom <= arg && rangeTo >= arg))
			{
				var res = await _deviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(rangeFrom,
					(ushort) (rangeTo - rangeFrom), "Read measuring");
				if (res.IsSuccessful)
				{
					for (var i = rangeFrom; i < rangeTo; i++)
					{
						if (_deviceContext.DeviceMemory.DeviceMemoryValues.ContainsKey(i))
						{
							_deviceContext.DeviceMemory.DeviceMemoryValues[i] = res.Result[i - rangeFrom];
						}
						else
						{
							_deviceContext.DeviceMemory.DeviceMemoryValues.Add(i, res.Result[i - rangeFrom]);
						}
					}
				}
			}
		}


		//private Task ExecuteLoadAllElements()
		//{

		//}
		//private async Task ExecuteLoadDescretMeasuringElement(DescretMeasuringElement descretMeasuringElement)
		//{
		//	if ((descretMeasuringElement.AddressOfBit.NumberOfFunction == 3) || (descretMeasuringElement.AddressOfBit.NumberOfFunction == 4))
		//	{
		//		IQueryResult<ushort[]> queryResult =
		//			await _dataProviderContainer.DataProvider.ReadHoldingResgistersAsync(descretMeasuringElement.AddressOfBit.Address, 1, "Read");
		//		if (queryResult.IsSuccessful)
		//		{
		//			_deviceEventsDispatcher.
		//		}
		//	}
		//	else if ((this.AddressOfBit.NumberOfFunction == 1) || (this.AddressOfBit.NumberOfFunction == 2))
		//	{
		//		IQueryResult<bool> boolQueryResult =
		//			await this._dataProvider.ReadCoilStatusAsync(this.AddressOfBit.Address, MeasuringKeys.READ_SINGLE_BIT_QUERY);
		//		if (boolQueryResult.IsSuccessful)
		//		{

		//			if (boolQueryResult.Result == this.DeviceValue) return;
		//			this.DeviceValue = boolQueryResult.Result;
		//			this.ElementChangedAction?.Invoke();
		//		}
		//	}
		//}
		//private Task ExecuteWriteControlSignal()
		//{
		//IQueryResult queryResult = null;
		//	if (this.WritingValueContext.NumberOfFunction == 16)

		//{
		//	queryResult = await this._dataProvider.WriteMultipleRegistersAsync(this.WritingValueContext.Address,
		//		new[] { this.WritingValueContext.ValueToWrite }, "Write");

		//}
		//else if (this.WritingValueContext.NumberOfFunction == 5)

		//{
		//	queryResult = await this._dataProvider.WriteSingleCoilAsync(this.WritingValueContext.Address,
		//		this.WritingValueContext.ValueToWrite != 0, "Write");

		//}
		//if (queryResult != null)
		//{
		//	return queryResult.IsSuccessful;
		//}
		//return false;
		//}
		//private Task ExecuteLoadAllElements()
		//{

		//}


	}
}
