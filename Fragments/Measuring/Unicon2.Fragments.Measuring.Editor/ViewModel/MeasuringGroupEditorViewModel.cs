using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Editor.View.PresentationSettings;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel
{
    public class MeasuringGroupEditorViewModel : ViewModelBase, IMeasuringGroupEditorViewModel, IDropTarget
    {
        private readonly IMeasuringElementEditorViewModelFactory _measuringElementEditorViewModelFactory;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;
        private readonly ILogService _logService;
        private string _header;
        private int _discretGroupElementsCount;
        private string _discretGroupName;
        private ushort _discretGroupStartAddress;
        private ushort _discretGroupStartingBit;

        private static int FUNCTION_CODE = 3;
        private PresentationSettingsViewModel _presentationSettingsViewModel;

        public MeasuringGroupEditorViewModel(IMeasuringElementEditorViewModelFactory measuringElementEditorViewModelFactory,IApplicationGlobalCommands applicationGlobalCommands, ILogService logService)
        {
            _measuringElementEditorViewModelFactory = measuringElementEditorViewModelFactory;
            _applicationGlobalCommands = applicationGlobalCommands;
            _logService = logService;
            MeasuringElementEditorViewModels = new ObservableCollection<IMeasuringElementEditorViewModel>();
            AddDiscretMeasuringElementCommand = new RelayCommand(OnAddDiscretMeasuringElementExecute);
            AddDiscretMeasuringElementGroupCommand = new RelayCommand(OnAddDiscretMeasuringElementGroupCommandExecute);
            AddAnalogMeasuringElementCommand = new RelayCommand(OnAddAnalogMeasuringElementExecute);
            DeleteMeasuringElementCommand = new RelayCommand<object>(OnDeleteMeasuringElementExecute);
            AddControlSignalCommand = new RelayCommand(OnAddControlSignalExecute);
            OpenPresentationSettingsCommand = new RelayCommand(OnOpenPresentationSettingsCommand);
            PresentationSettingsViewModel = new PresentationSettingsViewModel(this,new Dictionary<Guid, PositioningInfoViewModel>());
            AddDateTimeCommand=new RelayCommand(OnAddDateTimeExecute);
        }

        private void OnAddDateTimeExecute()
        {
            MeasuringElementEditorViewModels.Add(_measuringElementEditorViewModelFactory.CreateDateTimeEditorViewModel());
        }

        private void OnAddControlSignalExecute()
        {
            MeasuringElementEditorViewModels.Add(_measuringElementEditorViewModelFactory.CreateControlSignalEditorViewModel());
        }


        private void OnDeleteMeasuringElementExecute(object obj)
        {
            if (obj is IMeasuringElementEditorViewModel)
            {
                MeasuringElementEditorViewModels.Remove(obj as IMeasuringElementEditorViewModel);
            }
        }
        private void OnOpenPresentationSettingsCommand()
        {
            PresentationSettingsViewModel.UpdateMeasuringElements();
            _applicationGlobalCommands.ShowWindowModal(() => new PresentationSettingsWindow(),
		        PresentationSettingsViewModel);
        }
        
        private void OnAddAnalogMeasuringElementExecute()
        {
            MeasuringElementEditorViewModels.Add(_measuringElementEditorViewModelFactory.CreateAnalogMeasuringElementEditorViewModel());
        }

        private void OnAddDiscretMeasuringElementExecute()
        {
            MeasuringElementEditorViewModels.Add(_measuringElementEditorViewModelFactory.CreateDiscretMeasuringElementEditorViewModel());
        }

        private void OnAddDiscretMeasuringElementGroupCommandExecute()
        {
            try
            {
                for (int i = 0; i < DiscretGroupElementsCount; i++)
                {
                    if (DiscretGroupStartingBit > 15)
                    {
                        DiscretGroupStartAddress++;
                        DiscretGroupStartingBit = 0;
                    }

                    var item =
                        _measuringElementEditorViewModelFactory.CreateDiscretMeasuringElementEditorViewModel() as
                            IDiscretMeasuringElementEditorViewModel;
                    item.BitAddressEditorViewModel.Address = DiscretGroupStartAddress;
                    item.BitAddressEditorViewModel.BitNumberInWord = DiscretGroupStartingBit;
                    item.BitAddressEditorViewModel.FunctionNumber = FUNCTION_CODE;
                    item.Header = DiscretGroupName + " " + (i + 1);

                    MeasuringElementEditorViewModels.Add(item);

                    DiscretGroupStartingBit++;

                }

            }
            catch (Exception ex)
            {
                _logService.LogMessage(ex.Message);
            }
            finally
            {

            }
        }


        public string StrongName => MeasuringKeys.MEASURING_GROUP;

     
     

        public string Header
        {
            get { return _header; }
            set
            {
                if (value == String.Empty) return;
                _header = value;
                RaisePropertyChanged();
            }
        }

        public int DiscretGroupElementsCount
        {
            get { return _discretGroupElementsCount; }
            set
            {
                _discretGroupElementsCount = value;
                RaisePropertyChanged();
            }
        }
        public string DiscretGroupName
        {
            get { return _discretGroupName; }
            set
            {
                _discretGroupName = value;
                RaisePropertyChanged();
            }
        }
        public ushort DiscretGroupStartAddress
        {
            get { return _discretGroupStartAddress; }
            set
            {
                _discretGroupStartAddress = value;
                RaisePropertyChanged();
            }
        }
        public ushort DiscretGroupStartingBit
        {
            get { return _discretGroupStartingBit; }
            set
            {
                _discretGroupStartingBit = value;
                RaisePropertyChanged();
            }
        }

        public PresentationSettingsViewModel PresentationSettingsViewModel
        {
	        get => _presentationSettingsViewModel;
	        set
	        {
		        _presentationSettingsViewModel = value;
		        RaisePropertyChanged();
	        }
        }
        public ObservableCollection<IMeasuringElementEditorViewModel> MeasuringElementEditorViewModels { get; set; }
        public ICommand AddAnalogMeasuringElementCommand { get; }
        public ICommand AddDiscretMeasuringElementCommand { get; }
        public ICommand AddDiscretMeasuringElementGroupCommand { get; }
        public ICommand AddDateTimeCommand { get; }

        public ICommand AddControlSignalCommand { get; }
        public ICommand OpenPresentationSettingsCommand { get; }

        public ICommand DeleteMeasuringElementCommand { get; }


        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is IMeasuringElementEditorViewModel)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {

        }
    }
}
