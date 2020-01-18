using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel
{
    public class MeasuringGroupEditorViewModel : ViewModelBase, IMeasuringGroupEditorViewModel, IDropTarget
    {
        private readonly IMeasuringElementEditorViewModelFactory _measuringElementEditorViewModelFactory;
        private string _header;
        private IMeasuringGroup _measuringGroup;
        private int _discretGroupElementsCount;
        private string _discretGroupName;
        private ushort _discretGroupStartAddress;
        private ushort _discretGroupStartingBit;

        #region [CONST]
        private static int FUNCTION_CODE = 3;
        #endregion

        public MeasuringGroupEditorViewModel(IMeasuringElementEditorViewModelFactory measuringElementEditorViewModelFactory)
        {
            this._measuringElementEditorViewModelFactory = measuringElementEditorViewModelFactory;
            this.MeasuringElementEditorViewModels = new ObservableCollection<IMeasuringElementEditorViewModel>();
            this.AddDiscretMeasuringElementCommand = new RelayCommand(this.OnAddDiscretMeasuringElementExecute);
            this.AddDiscretMeasuringElementGroupCommand = new RelayCommand(this.OnAddDiscretMeasuringElementGroupCommandExecute);
            this.AddAnalogMeasuringElementCommand = new RelayCommand(this.OnAddAnalogMeasuringElementExecute);
            this.DeleteMeasuringElementCommand = new RelayCommand<object>(this.OnDeleteMeasuringElementExecute);
            this.AddControlSignalCommand = new RelayCommand(this.OnAddControlSignalExecute);
        }

        private void OnAddControlSignalExecute()
        {
            this.MeasuringElementEditorViewModels.Add(this._measuringElementEditorViewModelFactory.CreateControlSignalEditorViewModel());
        }


        private void OnDeleteMeasuringElementExecute(object obj)
        {
            if (obj is IMeasuringElementEditorViewModel)
            {
                this.MeasuringElementEditorViewModels.Remove(obj as IMeasuringElementEditorViewModel);
            }
        }


        private void OnAddAnalogMeasuringElementExecute()
        {
            this.MeasuringElementEditorViewModels.Add(this._measuringElementEditorViewModelFactory.CreateAnalogMeasuringElementEditorViewModel());
        }

        private void OnAddDiscretMeasuringElementExecute()
        {
            this.MeasuringElementEditorViewModels.Add(this._measuringElementEditorViewModelFactory.CreateDiscretMeasuringElementEditorViewModel());
        }

        private void OnAddDiscretMeasuringElementGroupCommandExecute()
        {
            try
            {
                for (int i = 0; i < DiscretGroupElementsCount; i++)
                {
                    if(DiscretGroupStartingBit > 15)
                    {
                        DiscretGroupStartAddress++;
                        DiscretGroupStartingBit = 0;
                    }
                    var item = this._measuringElementEditorViewModelFactory.CreateDiscretMeasuringElementEditorViewModel() as IDiscretMeasuringElementEditorViewModel;
                    item.BitAddressEditorViewModel.Address = DiscretGroupStartAddress;
                    item.BitAddressEditorViewModel.BitNumberInWord = DiscretGroupStartingBit;
                    item.BitAddressEditorViewModel.FunctionNumber = FUNCTION_CODE;
                    item.Header = DiscretGroupName + " " + (i + 1);

                    this.MeasuringElementEditorViewModels.Add(item);

                    DiscretGroupStartingBit++;

                }

            }
            catch (Exception ex) { }
            finally
            {

            }
        }


        #region Implementation of IStronglyNamed

        public string StrongName => MeasuringKeys.MEASURING_GROUP;
        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value as IMeasuringGroup); }
        }

        private void SetModel(IMeasuringGroup measuringGroup)
        {
            this._measuringGroup = measuringGroup;
            this.MeasuringElementEditorViewModels.Clear();
            foreach (IMeasuringElement measuringElement in this._measuringGroup.MeasuringElements)
            {
                this.MeasuringElementEditorViewModels.Add(this._measuringElementEditorViewModelFactory.CreateMeasuringElementEditorViewModel(measuringElement));
            }
            this.Header = this._measuringGroup.Name;
        }

        private IMeasuringGroup GetModel()
        {

            this._measuringGroup.Name = this.Header;
            this._measuringGroup.MeasuringElements.Clear();
            foreach (IMeasuringElementEditorViewModel measuringElementEditorViewModel in this.MeasuringElementEditorViewModels)
            {
                this._measuringGroup.MeasuringElements.Add(measuringElementEditorViewModel.Model as IMeasuringElement);
            }
            return this._measuringGroup;
        }



        #endregion

        #region Implementation of IMeasuringGroupEditorViewModel

        public string Header
        {
            get { return this._header; }
            set
            {
                if (value == String.Empty) return;
                this._header = value;
                this.RaisePropertyChanged();
            }
        }

        public int DiscretGroupElementsCount
        {
            get { return this._discretGroupElementsCount; }
            set
            {
                this._discretGroupElementsCount = value;
                RaisePropertyChanged();
            }
        }
        public string DiscretGroupName
        {
            get { return this._discretGroupName; }
            set
            {
                this._discretGroupName = value;
                RaisePropertyChanged();
            }
        }
        public ushort DiscretGroupStartAddress
        {
            get { return this._discretGroupStartAddress; }
            set
            {
                this._discretGroupStartAddress = value;
                RaisePropertyChanged();
            }
        }
        public ushort DiscretGroupStartingBit
        {
            get { return this._discretGroupStartingBit; }
            set
            {
                this._discretGroupStartingBit = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<IMeasuringElementEditorViewModel> MeasuringElementEditorViewModels { get; set; }
        public ICommand AddAnalogMeasuringElementCommand { get; }
        public ICommand AddDiscretMeasuringElementCommand { get; }
        public ICommand AddDiscretMeasuringElementGroupCommand { get; }
        public ICommand AddControlSignalCommand { get; }

        public ICommand DeleteMeasuringElementCommand { get; }

        #endregion


        #region Implementation of IDropTarget

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

        #endregion
    }
}
