using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
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

        public AnalogMeasuringElementEditorViewModel(IFormatterEditorFactory formatterEditorFactory)
        {
            this._formatterEditorFactory = formatterEditorFactory;
            this.ShowFormatterParametersCommand = new RelayCommand(this.OnShowFormatterParametersExecute);
        }

        private void OnShowFormatterParametersExecute()
        {
            this._formatterEditorFactory.EditFormatterByUser(this._measuringElement as IUshortFormattable);
            this.RaisePropertyChanged(nameof(this.FormatterString));
        }


        #region Implementation of IMeasurable

        public string MeasureUnit
        {
            get { return this._measureUnit; }
            set
            {
                this._measureUnit = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsMeasureUnitEnabled
        {
            get { return this._isMeasureUnitEnabled; }
            set
            {
                this._isMeasureUnitEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Implementation of IAnalogMeasuringElementEditorViewModel

        public ICommand ShowFormatterParametersCommand { get; }

        public ushort Address
        {
            get { return this._address; }
            set
            {
                this._address = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort NumberOfPoints
        {
            get { return this._numberOfPoints; }
            set
            {
                this._numberOfPoints = value;
                this.RaisePropertyChanged();
            }
        }

        public string FormatterString
        {
            get { return (this._measuringElement as IAnalogMeasuringElement)?.UshortsFormatter?.StrongName; }
        }

        #endregion

        #region Overrides of MeasuringElementEditorViewModelBase

        public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion


        #region Overrides of MeasuringElementEditorViewModelBase

        protected override void SetModel(object value)
        {
            base.SetModel(value);
            this.Address = (this._measuringElement as IAnalogMeasuringElement).Address;
            this.NumberOfPoints = (this._measuringElement as IAnalogMeasuringElement).NumberOfPoints;
            this.MeasureUnit = (this._measuringElement as IAnalogMeasuringElement).MeasureUnit;
            this.IsMeasureUnitEnabled = (this._measuringElement as IAnalogMeasuringElement).IsMeasureUnitEnabled;
        }



        protected override IMeasuringElement GetModel()
        {
            (this._measuringElement as IAnalogMeasuringElement).Address = this.Address;
            (this._measuringElement as IAnalogMeasuringElement).NumberOfPoints = this.NumberOfPoints;
            (this._measuringElement as IAnalogMeasuringElement).MeasureUnit = this.MeasureUnit;
            (this._measuringElement as IAnalogMeasuringElement).IsMeasureUnitEnabled = this.IsMeasureUnitEnabled;
            return base.GetModel();
        }

        public override string NameForUiKey => MeasuringKeys.ANALOG_MEASURING_ELEMENT;

        #endregion
    }
}
