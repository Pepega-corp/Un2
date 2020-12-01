using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Fragments.ModbusMemory.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.ModbusMemory.ViewModels
{
	public class ModbusMemoryViewModel : DisposableBindableBase, IModbusMemoryViewModel
	{
		private ObservableCollection<IModbusMemoryEntityViewModel> _modbusMemoryEntityViewModels;
		private IModbusMemorySettingsViewModel _modbusMemorySettingsViewModel;
		private readonly ITypesContainer _container;
		private readonly Func<IMemoryBitViewModel> _memoryBitViewModelGettingFunc;
		private readonly Func<IModbusMemoryEntity> _modbusMemoryEntityGettingFunc;
		private readonly IApplicationGlobalCommands _applicationGlobalCommands;
		private bool _isQueriesStarted;
		private bool _isQueriesStartedError;
		private string _queriesError;
		private List<IModbusConversionParametersViewModel> _modbusConversionParametersViewModels;
		private IModbusMemory _modbusMemory;
		private IFragmentOptionsViewModel _fragmentOptionsViewModel;


		public ModbusMemoryViewModel(IModbusMemorySettingsViewModel modbusMemorySettingsViewModel,
			ITypesContainer container, Func<IMemoryBitViewModel> memoryBitViewModelGettingFunc,
			Func<IModbusMemoryEntity> modbusMemoryEntityGettingFunc,
			IApplicationGlobalCommands applicationGlobalCommands)
		{
			_container = container;
			_memoryBitViewModelGettingFunc = memoryBitViewModelGettingFunc;
			_modbusMemoryEntityGettingFunc = modbusMemoryEntityGettingFunc;
			_applicationGlobalCommands = applicationGlobalCommands;
			ModbusMemoryEntityViewModels = new ObservableCollection<IModbusMemoryEntityViewModel>();
			_modbusConversionParametersViewModels = new List<IModbusConversionParametersViewModel>(32);
			ModbusMemorySettingsViewModel = modbusMemorySettingsViewModel;
			ExecuteOneQueryCommand = new RelayCommand(async () =>
			{
				if (DeviceContext.DataProviderContainer == null) return;
				await OnExecuteOneQuery();
			});
			EditEntityCommand = new RelayCommand<IModbusMemoryEntityViewModel>(OnExecuteEditEntity);
			SetTrueBitCommand = new RelayCommand<object>(OnSetTrueBitExecute);
			SetFalseBitCommand = new RelayCommand<object>(OnSetFalseBitExecute);

		}

		private async void OnSetFalseBitExecute(object obj)
		{
			IMemoryBitViewModel memoryBitViewModel = obj as IMemoryBitViewModel;
			await DeviceContext.DataProviderContainer.DataProvider.Item.WriteSingleCoilAsync(memoryBitViewModel.Address, false,
				ApplicationGlobalNames.QueriesNames.WRITE_MODBUS_MEMORY_QUERY_KEY);
			await OnExecuteOneQuery();
		}

		private async void OnSetTrueBitExecute(object obj)
		{
			IMemoryBitViewModel memoryBitViewModel = obj as IMemoryBitViewModel;
			await DeviceContext.DataProviderContainer.DataProvider.Item.WriteSingleCoilAsync(memoryBitViewModel.Address, true,
				ApplicationGlobalNames.QueriesNames.WRITE_MODBUS_MEMORY_QUERY_KEY);
			await OnExecuteOneQuery();
		}

		private async void OnExecuteEditEntity(IModbusMemoryEntityViewModel modbusMemoryEntityViewModel)
		{

			IModbusEntityEditingViewModel modbusEntityEditingViewModel =
				_container.Resolve<IModbusEntityEditingViewModel>();
			modbusEntityEditingViewModel.DataProviderContainer = DeviceContext.DataProviderContainer;
			modbusEntityEditingViewModel.SetEntity(modbusMemoryEntityViewModel.Clone() as IModbusMemoryEntityViewModel);
			_applicationGlobalCommands.ShowWindowModal(() => new ModbusEntityEditingView(),
				modbusEntityEditingViewModel);
			if (!IsQueriesStarted) await OnExecuteOneQuery();
		}


		public ObservableCollection<IModbusMemoryEntityViewModel> ModbusMemoryEntityViewModels
		{
			get { return _modbusMemoryEntityViewModels; }
			set
			{
				_modbusMemoryEntityViewModels = value;
				RaisePropertyChanged();
			}
		}

		public IModbusMemorySettingsViewModel ModbusMemorySettingsViewModel
		{
			get { return _modbusMemorySettingsViewModel; }
			set
			{
				_modbusMemorySettingsViewModel = value;
				_modbusConversionParametersViewModels = new List<IModbusConversionParametersViewModel>(32);
				_modbusMemorySettingsViewModel.ModbusMemorySettingsChanged += OnModbusMemorySettingsChanged;
				OnModbusMemorySettingsChanged(_modbusMemorySettingsViewModel.GetModbusMemorySettings());
				RaisePropertyChanged();
			}
		}

		private void OnModbusMemorySettingsChanged(IModbusMemorySettings modbusMemorySettings)
		{
			bool isQueriesWasStartedBefore = false;
			if (IsQueriesStarted)
			{
				isQueriesWasStartedBefore = true;
				IsQueriesStarted = false;
			}

			if (ModbusMemoryEntityViewModels.Count != modbusMemorySettings.NumberOfPoints)
			{
				ModbusMemoryEntityViewModels.Clear();
			}

			foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in ModbusMemoryEntityViewModels)
			{
				modbusMemoryEntityViewModel.SetViewSetOption(modbusMemorySettings.IsDiscretOption);
			}

			//всякие условия вокруг - оптимизация вывода, чтобы не перерисовывать все каждый раз
			for (int i = 0; i < modbusMemorySettings.NumberOfPoints; i++)
			{

				if (_modbusConversionParametersViewModels.Count < i + 1)
				{
					_modbusConversionParametersViewModels.Add(
						_container.Resolve<IModbusConversionParametersViewModel>());
				}

				if (ModbusMemoryEntityViewModels.Count <= i)
				{
					ModbusMemoryEntityViewModel modbusMemoryEntityViewModel =
						new ModbusMemoryEntityViewModel(_memoryBitViewModelGettingFunc,
							_modbusMemoryEntityGettingFunc,
							_modbusConversionParametersViewModels[i]);
					modbusMemoryEntityViewModel.SetViewSetOption(modbusMemorySettings.IsDiscretOption);
					modbusMemoryEntityViewModel.SetAddress(modbusMemorySettings.BaseAdress + i);
					ModbusMemoryEntityViewModels.Add(modbusMemoryEntityViewModel);
				}
				else
				{
					ModbusMemoryEntityViewModels[i].SetViewSetOption(modbusMemorySettings.IsDiscretOption);
					ModbusMemoryEntityViewModels[i].SetAddress(modbusMemorySettings.BaseAdress + i);
				}
			}

			if (isQueriesWasStartedBefore)
				IsQueriesStarted = true;
			else
			{
				ModbusMemoryEntityViewModels.ForEach((model => model.SetError()));
			}
		}

		public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;


		public IFragmentOptionsViewModel FragmentOptionsViewModel
		{
			get { return _fragmentOptionsViewModel; }
			set
			{
				_fragmentOptionsViewModel = value;
				RaisePropertyChanged();
			}
		}

		public bool IsQueriesStarted
		{
			get { return _isQueriesStarted; }
			set
			{
				if (DeviceContext.DataProviderContainer.DataProvider == null) return;
				_isQueriesStarted = value;
				if (value)
				{
					StartUpdating();
				}

				RaisePropertyChanged();
			}
		}

		// Мои изменения
		public bool IsQueriesStartedError
		{
			get { return _isQueriesStartedError; }
			set
			{
				_isQueriesStartedError = value;
				RaisePropertyChanged();
			}
		}

		// Мои изменения
		public string QueriesError
		{
			get { return _queriesError; }
			set
			{
				_queriesError = value;
				RaisePropertyChanged();
			}
		}


		public ICommand ExecuteOneQueryCommand { get; }

		public ICommand EditEntityCommand { get; }

		public ICommand SetTrueBitCommand { get; }

		public ICommand SetFalseBitCommand { get; }



		private async Task OnExecuteOneQuery()
		{
            if (DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                IModbusMemorySettings modbusMemorySettings = _modbusMemorySettingsViewModel.GetModbusMemorySettings();
                if (modbusMemorySettings.IsDiscretOption)
                {
                    IQueryResult<bool[]> queryResult =
                        await DeviceContext.DataProviderContainer.DataProvider.Item.ReadCoilStatusAsync(
                            (ushort) modbusMemorySettings.BaseAdress,
                            ApplicationGlobalNames.QueriesNames.MODBUS_MEMORY_QUERY_KEY,
                            (ushort) (modbusMemorySettings.NumberOfPoints * 16));
                    UpdateEntities(queryResult);
                }
                else
                {
                    IQueryResult<ushort[]> queryResult =
                        await DeviceContext.DataProviderContainer.DataProvider.Item.ReadHoldingResgistersAsync(
                            (ushort) modbusMemorySettings.BaseAdress,
                            (ushort) modbusMemorySettings.NumberOfPoints,
                            ApplicationGlobalNames.QueriesNames.MODBUS_MEMORY_QUERY_KEY);
                    UpdateEntities(queryResult);
                }
            }
        }

        private async void StartUpdating()
        {
            while (true)
            {
                if (IsQueriesStarted && DeviceContext.DataProviderContainer.DataProvider.IsSuccess)
                {
                    if (!DeviceContext.DataProviderContainer.DataProvider.Item.LastQuerySucceed)
                    {
                        IsQueriesStarted = false;
                        QueriesError = "Последний запрос не успешен!"; // Мои изменения
                        IsQueriesStartedError = true; // Мои изменения
                    }
                    // Мои изменения
                    else
                    {
                        IsQueriesStartedError = false;
                        QueriesError = null;
                    }

                    // End Мои изменения
                    await OnExecuteOneQuery();
                }
                else
                {
                    return;
                }
            }
        }

        private void UpdateEntities(IQueryResult<ushort[]> queryResult)
		{
			if (!queryResult.IsSuccessful)
			{
				foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in ModbusMemoryEntityViewModels)
				{
					modbusMemoryEntityViewModel.SetError();
				}

				return;
			}

			int index = 0;
			foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in ModbusMemoryEntityViewModels)
			{
				if (queryResult.Result.Length > index)
				{
					modbusMemoryEntityViewModel.SetUshortValue((queryResult.Result)[index]);
					
				}
				index++;
			}
		}

		private void UpdateEntities(IQueryResult<bool[]> queryResult)
		{
			if (!queryResult.IsSuccessful)
			{
				foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in ModbusMemoryEntityViewModels)
				{
					modbusMemoryEntityViewModel.SetError();
				}

				return;
			}

			int index = 0;
			foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in ModbusMemoryEntityViewModels)
			{
				BitArray bitArray = new BitArray(queryResult.Result.Skip(index * 16).Take(16).ToArray());

				modbusMemoryEntityViewModel.SetUshortValue((ushort) bitArray.GetIntFromBitArray());
				index++;
			}
		}

		public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY +
		                            ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

		public object Model
		{
			get { return _modbusMemory; }
			set
			{
				if (_modbusMemory != value)
					_modbusMemory?.Dispose();
				_modbusMemory = value as IModbusMemory;
			}
		}


		protected override void OnDisposing()
		{
			IsQueriesStarted = false;
			base.OnDisposing();
		}

		public void Initialize(IDeviceFragment deviceFragment)
		{
			
		}

		public DeviceContext DeviceContext { get; set; }
	}
}