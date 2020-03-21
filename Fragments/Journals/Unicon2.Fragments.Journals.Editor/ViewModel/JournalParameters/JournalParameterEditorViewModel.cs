using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters
{
    public class JournalParameterEditorViewModel : ViewModelBase, IJournalParameterEditorViewModel
    {
        protected IJournalParameter _journalParameter;
        private bool _isMeasureUnitEnabled;
        private bool _isInEditMode;
        private string _header;
        private string _address;
        private string _numberOfPoints;

        public JournalParameterEditorViewModel(IJournalParameter journalParameter)
        {
            this._journalParameter = journalParameter;
        }

        public bool IsInEditMode
        {
            get { return this._isInEditMode; }
            set
            {
                this._isInEditMode = value;
                this.RaisePropertyChanged();
            }
        }

        public virtual void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public virtual void StopEditElement()
        {
            this.SaveModel();
            this.RaisePropertyChanged(nameof(this.FormatterString));
            this.IsInEditMode = false;
        }

        public string StrongName => JournalKeys.JOURNAL_PARAMETER +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        protected virtual void SetModel(object value)
        {
            IJournalParameter journalParameter = value as IJournalParameter;
            if (journalParameter == null) return;
            this._journalParameter = journalParameter;

            this.Address = journalParameter.StartAddress.ToString();
            this.Header = journalParameter.Name;
            this.NumberOfPoints = journalParameter.NumberOfPoints.ToString();
            this.MeasureUnit = this._journalParameter.MeasureUnit;
            this.IsMeasureUnitEnabled = this._journalParameter.IsMeasureUnitEnabled;
            this.RaisePropertyChanged(nameof(this.FormatterString));
            FormatterParametersViewModel = StaticContainer.Container.Resolve<IFormatterViewModelFactory>()
                .CreateFormatterViewModel(_journalParameter.UshortsFormatter);
        }

        private object GetModel()
        {
            _journalParameter.UshortsFormatter = StaticContainer.Container.Resolve<ISaveFormatterService>()
                .CreateUshortsParametersFormatter(FormatterParametersViewModel);
            return this._journalParameter;
        }

        protected virtual void SaveModel()
        {
            this._journalParameter.StartAddress = ushort.Parse(this.Address);
            this._journalParameter.Name = this.Header;
            this._journalParameter.NumberOfPoints = ushort.Parse(this.NumberOfPoints);
            this._journalParameter.IsMeasureUnitEnabled = this.IsMeasureUnitEnabled;
            this._journalParameter.MeasureUnit = this.MeasureUnit;
        }

        public string MeasureUnit { get; set; }

        public bool IsMeasureUnitEnabled
        {
            get { return this._isMeasureUnitEnabled; }
            set
            {
                this._isMeasureUnitEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public string Header
        {
            get { return this._header; }
            set
            {
                this._header = value;
                this.RaisePropertyChanged();

            }
        }

        public string Address
        {
            get { return this._address; }
            set
            {
                this._address = value;
                this.RaisePropertyChanged();

            }
        }

        public string NumberOfPoints
        {
            get { return this._numberOfPoints; }
            set
            {
                this._numberOfPoints = value;
                this.RaisePropertyChanged();

            }
        }

        public string FormatterString => FormatterParametersViewModel?.RelatedUshortsFormatterViewModel?.StrongName;

        public string Name { get; set; }
        public IFormatterParametersViewModel FormatterParametersViewModel { get; set; }
    }
}
