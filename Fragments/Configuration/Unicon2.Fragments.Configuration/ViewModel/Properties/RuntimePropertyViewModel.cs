using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimePropertyViewModel : RuntimeConfigurationItemViewModelBase, IRuntimePropertyViewModel,ICanBeHidden
    {
        private IFormattedValueViewModel _value;
        private IEditableValueViewModel _localValue;
        private string _measureUnit;
        private bool _isMeasureUnitEnabled;
        private IRangeViewModel _rangeViewModel;
        private bool _isRangeEnabled;
        private bool _isHidden;

        public RuntimePropertyViewModel()
        {
            IsCheckable = false;
            BitNumbersInWord=new ObservableCollection<IBitViewModel>();
            for (int i = 15; i >= 0; i--)
            {
                IBitViewModel bitViewModel = new BitViewModel(i, true);
                BitNumbersInWord.Add(bitViewModel);
            }
        }

        public IFormattedValueViewModel DeviceValue
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public IEditableValueViewModel LocalValue
        {
            get { return _localValue; }
            set
            {
                _localValue = value;
                RaisePropertyChanged();
            }
        }

        public override string TypeName => GetTypeName();

        public override string StrongName => ConfigurationKeys.RUNTIME_DEFAULT_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        protected virtual string GetTypeName()
        {
            return ConfigurationKeys.RUNTIME_DEFAULT_PROPERTY;
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

        public bool IsRangeEnabled
        {
            get { return _isRangeEnabled; }
            set
            {
                _isRangeEnabled = value;
                RaisePropertyChanged();
            }
        }

        public ushort Address { get; set; }
        public ushort NumberOfPoints { get; set; }

        public IRangeViewModel RangeViewModel
        {
            get { return _rangeViewModel; }
            set
            {
                _rangeViewModel = value;
                RaisePropertyChanged();
            }
        }

        public DeviceContext DeviceContext { get; set; }

        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                _isHidden = value;
                RaisePropertyChanged();
            }
        }

        public bool IsFromBits { get; set; }
        public ObservableCollection<IBitViewModel> BitNumbersInWord { get; set; }
        public (ushort address, ushort numberOfPoints) GetAddressInfo()
        {
            return (Address, NumberOfPoints);
        }
    }
}