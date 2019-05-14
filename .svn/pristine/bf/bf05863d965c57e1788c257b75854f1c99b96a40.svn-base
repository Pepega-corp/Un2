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
        
        public MeasuringGroupEditorViewModel(IMeasuringElementEditorViewModelFactory measuringElementEditorViewModelFactory)
        {
            this._measuringElementEditorViewModelFactory = measuringElementEditorViewModelFactory;
            this.MeasuringElementEditorViewModels = new ObservableCollection<IMeasuringElementEditorViewModel>();
            this.AddDiscretMeasuringElementCommand = new RelayCommand(this.OnAddDiscretMeasuringElementExecute);
            this.AddAnalogMeasuringElementCommand = new RelayCommand(this.OnAddAnalogMeasuringElementExecute);
            this.DeleteMeasuringElementCommand = new RelayCommand<object>(this.OnDeleteMeasuringElementExecute);
            this.AddControlSignalCommand = new RelayCommand<object>(this.OnAddControlSignalExecute);
        }

        private void OnAddControlSignalExecute(object obj)
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

        public ObservableCollection<IMeasuringElementEditorViewModel> MeasuringElementEditorViewModels { get; set; }
        public ICommand AddAnalogMeasuringElementCommand { get; }
        public ICommand AddDiscretMeasuringElementCommand { get; }

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
