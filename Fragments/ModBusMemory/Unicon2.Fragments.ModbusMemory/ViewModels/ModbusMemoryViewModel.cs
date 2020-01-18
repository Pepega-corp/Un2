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
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
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
        private IDataProvider _dataProvider;
        private List<IModbusConversionParametersViewModel> _modbusConversionParametersViewModels;
        private IModbusMemory _modbusMemory;
        private IFragmentOptionsViewModel _fragmentOptionsViewModel;


        public ModbusMemoryViewModel(IModbusMemorySettingsViewModel modbusMemorySettingsViewModel,
            ITypesContainer container, Func<IMemoryBitViewModel> memoryBitViewModelGettingFunc,
            Func<IModbusMemoryEntity> modbusMemoryEntityGettingFunc, IApplicationGlobalCommands applicationGlobalCommands)
        {
            this._container = container;
            this._memoryBitViewModelGettingFunc = memoryBitViewModelGettingFunc;
            this._modbusMemoryEntityGettingFunc = modbusMemoryEntityGettingFunc;
            this._applicationGlobalCommands = applicationGlobalCommands;
            this.ModbusMemoryEntityViewModels = new ObservableCollection<IModbusMemoryEntityViewModel>();
            this._modbusConversionParametersViewModels = new List<IModbusConversionParametersViewModel>(32);
            this.ModbusMemorySettingsViewModel = modbusMemorySettingsViewModel;
            this.ExecuteOneQueryCommand = new RelayCommand(async () =>
            {
                if (this._dataProvider == null) return;
                await this.OnExecuteOneQuery();
            });
            this.EditEntityCommand = new RelayCommand<IModbusMemoryEntityViewModel>(this.OnExecuteEditEntity);
            this.SetTrueBitCommand = new RelayCommand<object>(this.OnSetTrueBitExecute);
            this.SetFalseBitCommand = new RelayCommand<object>(this.OnSetFalseBitExecute);

        }

        private async void OnSetFalseBitExecute(object obj)
        {
            IMemoryBitViewModel memoryBitViewModel = obj as IMemoryBitViewModel;
            await this._dataProvider.WriteSingleCoilAsync(memoryBitViewModel.Address, false,
                ApplicationGlobalNames.QueriesNames.WRITE_MODBUS_MEMORY_QUERY_KEY);
            await this.OnExecuteOneQuery();
        }

        private async void OnSetTrueBitExecute(object obj)
        {
            IMemoryBitViewModel memoryBitViewModel = obj as IMemoryBitViewModel;
            await this._dataProvider.WriteSingleCoilAsync(memoryBitViewModel.Address, true, ApplicationGlobalNames.QueriesNames.WRITE_MODBUS_MEMORY_QUERY_KEY);
            await this.OnExecuteOneQuery();
        }

        private void OnExecuteEditEntity(IModbusMemoryEntityViewModel modbusMemoryEntityViewModel)
        {

            IModbusEntityEditingViewModel modbusEntityEditingViewModel =
                this._container.Resolve<IModbusEntityEditingViewModel>();
            modbusEntityEditingViewModel.SetDataProvider(this._dataProvider);
            modbusEntityEditingViewModel.SetEntity(modbusMemoryEntityViewModel.Clone() as IModbusMemoryEntityViewModel);
            this._applicationGlobalCommands.ShowWindowModal(() => new ModbusEntityEditingView(),
                modbusEntityEditingViewModel);
            if (!this.IsQueriesStarted) this.OnExecuteOneQuery();
        }


        public ObservableCollection<IModbusMemoryEntityViewModel> ModbusMemoryEntityViewModels
        {
            get { return this._modbusMemoryEntityViewModels; }
            set
            {
                this._modbusMemoryEntityViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public IModbusMemorySettingsViewModel ModbusMemorySettingsViewModel
        {
            get { return this._modbusMemorySettingsViewModel; }
            set
            {
                this._modbusMemorySettingsViewModel = value;
                this._modbusConversionParametersViewModels = new List<IModbusConversionParametersViewModel>(32);
                this._modbusMemorySettingsViewModel.ModbusMemorySettingsChanged += this.OnModbusMemorySettingsChanged;
                this.OnModbusMemorySettingsChanged(this._modbusMemorySettingsViewModel.GetModbusMemorySettings());
                this.RaisePropertyChanged();
            }
        }

        private void OnModbusMemorySettingsChanged(IModbusMemorySettings modbusMemorySettings)
        {
            bool isQueriesWasStartedBefore = false;
            if (this.IsQueriesStarted)
            {
                isQueriesWasStartedBefore = true;
                this.IsQueriesStarted = false;
            }
            if (this.ModbusMemoryEntityViewModels.Count != modbusMemorySettings.NumberOfPoints)
            {
                this.ModbusMemoryEntityViewModels.Clear();
            }

            foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in this.ModbusMemoryEntityViewModels)
            {
                modbusMemoryEntityViewModel.SetViewSetOption(modbusMemorySettings.IsDiscretOption);
            }

            //всякие условия вокруг - оптимизация вывода, чтобы не перерисовывать все каждый раз
            for (int i = 0; i < modbusMemorySettings.NumberOfPoints; i++)
            {

                if (this._modbusConversionParametersViewModels.Count < i + 1)
                {
                    this._modbusConversionParametersViewModels.Add(
                        this._container.Resolve<IModbusConversionParametersViewModel>());
                }
                if (this.ModbusMemoryEntityViewModels.Count <= i)
                {
                    ModbusMemoryEntityViewModel modbusMemoryEntityViewModel =
                        new ModbusMemoryEntityViewModel(this._memoryBitViewModelGettingFunc,
                            this._modbusMemoryEntityGettingFunc,
                            this._modbusConversionParametersViewModels[i]);
                    this.ModbusMemoryEntityViewModels.Add(modbusMemoryEntityViewModel);
                }
                else
                {
                    this.ModbusMemoryEntityViewModels[i].SetViewSetOption(modbusMemorySettings.IsDiscretOption);
                    this.ModbusMemoryEntityViewModels[i].SetAddress(modbusMemorySettings.BaseAdress + i);
                }
            }
            if (isQueriesWasStartedBefore)
                this.IsQueriesStarted = true;
            else
            {
                this.ModbusMemoryEntityViewModels.ForEach((model => model.SetError()));
            }
        }

        public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public IFragmentOptionsViewModel FragmentOptionsViewModel
        {
            get { return this._fragmentOptionsViewModel; }
            set
            {
                this._fragmentOptionsViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsQueriesStarted
        {
            get { return this._isQueriesStarted; }
            set
            {
                if (this._dataProvider == null) return;
                this._isQueriesStarted = value;
                if (value)
                {
                    this.StartUpdating();
                }
                this.RaisePropertyChanged();
            }
        }

        // Мои изменения
        public bool IsQueriesStartedError
        {
            get { return this._isQueriesStartedError; }
            set
            {
                this._isQueriesStartedError = value;
                this.RaisePropertyChanged();
            }
        }

        // Мои изменения
        public string QueriesError
        {
            get { return this._queriesError; }
            set
            {
                this._queriesError = value;
                this.RaisePropertyChanged();
            }
        }


        public ICommand ExecuteOneQueryCommand { get; }

        public ICommand EditEntityCommand { get; }

        public ICommand SetTrueBitCommand { get; }

        public ICommand SetFalseBitCommand { get; }



        private async Task OnExecuteOneQuery()
        {
            IModbusMemorySettings modbusMemorySettings = this._modbusMemorySettingsViewModel.GetModbusMemorySettings();
            if (modbusMemorySettings.IsDiscretOption)
            {
                IQueryResult<bool[]> queryResult = await this._dataProvider.ReadCoilStatusAsync((ushort)modbusMemorySettings.BaseAdress,
                    ApplicationGlobalNames.QueriesNames.MODBUS_MEMORY_QUERY_KEY, (ushort)(modbusMemorySettings.NumberOfPoints * 16));
                this.UpdateEntities(queryResult);
            }
            else
            {
                IQueryResult<ushort[]> queryResult = await this._dataProvider.ReadHoldingResgistersAsync((ushort)modbusMemorySettings.BaseAdress,
                 (ushort)modbusMemorySettings.NumberOfPoints,
                 ApplicationGlobalNames.QueriesNames.MODBUS_MEMORY_QUERY_KEY);
                this.UpdateEntities(queryResult);
            }
        }

        private async void StartUpdating()
        {
            while (true)
            {
                if (this.IsQueriesStarted)
                {
                    if (!this._dataProvider.LastQuerySucceed)
                    {
                        this.IsQueriesStarted = false;
                        this.QueriesError = "Последний запрос не успешен!"; // Мои изменения
                        this.IsQueriesStartedError = true; // Мои изменения
                    }
                    // Мои изменения
                    else
                    {
                        this.IsQueriesStartedError = false;
                        this.QueriesError = null;
                    }
                    // End Мои изменения
                    await this.OnExecuteOneQuery();
                }
                else { return; }
            }
        }

        private void UpdateEntities(IQueryResult<ushort[]> queryResult)
        {
            if (!queryResult.IsSuccessful)
            {
                foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in this.ModbusMemoryEntityViewModels)
                {
                    modbusMemoryEntityViewModel.SetError();
                }
                return;
            }
            int index = 0;
            foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in this.ModbusMemoryEntityViewModels)
            {
                modbusMemoryEntityViewModel.SetUshortValue((queryResult.Result)[index]);
                index++;
            }
        }

        private void UpdateEntities(IQueryResult<bool[]> queryResult)
        {
            if (!queryResult.IsSuccessful)
            {
                foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in this.ModbusMemoryEntityViewModels)
                {
                    modbusMemoryEntityViewModel.SetError();
                }
                return;
            }
            int index = 0;
            foreach (IModbusMemoryEntityViewModel modbusMemoryEntityViewModel in this.ModbusMemoryEntityViewModels)
            {
                BitArray bitArray = new BitArray(queryResult.Result.Skip(index * 16).Take(16).ToArray());

                modbusMemoryEntityViewModel.SetUshortValue((ushort)bitArray.GetIntFromBitArray());
                index++;
            }
        }

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return this._modbusMemory; }
            set
            {
                if (this._modbusMemory != value)
                    this._modbusMemory?.Dispose();
                (value as IModbusMemory).DataProviderChanged += this.SetDataProvider;
                this._modbusMemory = value as IModbusMemory;
                this._dataProvider = this._modbusMemory?.GetDataProvider();
            }
        }


        protected override void OnDisposing()
        {
            this.IsQueriesStarted = false;
            base.OnDisposing();
        }
    }
}