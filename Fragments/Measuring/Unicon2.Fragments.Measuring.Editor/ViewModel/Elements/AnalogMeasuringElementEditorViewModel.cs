using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public class AnalogMeasuringElementEditorViewModel : MeasuringElementEditorViewModelBase, IAnalogMeasuringElementEditorViewModel
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private string _measureUnit;
        private bool _isMeasureUnitEnabled;
        private ushort _address;
        private ushort _numberOfPoints;
        private IFormatterParametersViewModel _formatterParametersViewModel;

        public AnalogMeasuringElementEditorViewModel(IFormatterEditorFactory formatterEditorFactory)
        {
            _formatterEditorFactory = formatterEditorFactory;
            ShowFormatterParametersCommand = new RelayCommand(OnShowFormatterParametersExecute);
        }

        private void OnShowFormatterParametersExecute()
        {
            _formatterEditorFactory.EditFormatterByUser(this);
           // this.RaisePropertyChanged(nameof(this.FormatterString));
        }


        public string MeasureUnit
        {
            get { return _measureUnit; }
            set
            {
                _measureUnit = value;
                RaisePropertyChanged();
            }
        }

        public bool IsMeasureUnitEnabled
        {
            get { return _isMeasureUnitEnabled; }
            set
            {
                _isMeasureUnitEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ShowFormatterParametersCommand { get; }

        public ushort Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }

        public ushort NumberOfPoints
        {
            get { return _numberOfPoints; }
            set
            {
                _numberOfPoints = value;
                RaisePropertyChanged();
            }
        }

		public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

		

        public override string NameForUiKey => MeasuringKeys.ANALOG_MEASURING_ELEMENT;
        public string Name { get; set; }
        public IFormatterParametersViewModel FormatterParametersViewModel
        {
	        get => _formatterParametersViewModel;
	        set
	        {
		        _formatterParametersViewModel = value;
		        RaisePropertyChanged();
	        }
        }
    }
}
