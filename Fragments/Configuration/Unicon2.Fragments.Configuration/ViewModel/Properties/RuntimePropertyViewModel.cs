using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Events;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimePropertyViewModel : RuntimeConfigurationItemViewModelBase, IRuntimePropertyViewModel
    {
        private IFormattedValueViewModel _value;
        private IEditableValueViewModel _localValue;
        private string _measureUnit;
        private bool _isMeasureUnitEnabled;
        private IRange _range;
        private IRangeViewModel _rangeViewModel;
        private bool _isRangeEnabled;

        public RuntimePropertyViewModel()
        {
            this.IsCheckable = false;
        }

        public IFormattedValueViewModel DeviceValue
        {
            get { return this._value; }
            set
            {
                this._value = value;
                this.RaisePropertyChanged();
            }
        }

        public IEditableValueViewModel LocalValue
        {
            get { return this._localValue; }
            set
            {
                this._localValue = value;
                this.RaisePropertyChanged();
            }
        }

        public override string TypeName => this.GetTypeName();

        protected virtual string GetTypeName()
        {
            return ConfigurationKeys.RUNTIME_DEFAULT_PROPERTY;
        }

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

        public bool IsRangeEnabled
        {
            get { return this._isRangeEnabled; }
            set
            {
                this._isRangeEnabled = value;
                this.RaisePropertyChanged();
            }
        }

        public IRangeViewModel RangeViewModel
        {
            get { return this._rangeViewModel; }
            set
            {
                this._rangeViewModel = value;
                this.RaisePropertyChanged();
            }
        }
        
    }
}