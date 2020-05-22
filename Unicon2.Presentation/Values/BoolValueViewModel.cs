using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class BoolValueViewModel : FormattableValueViewModelBase, IBoolValueViewModel
    {
        private bool _boolValueProperty;

        public override string StrongName => nameof(BoolValueViewModel);
        public override string AsString()
        {
	        return BoolValueProperty.ToString();
        }

        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value;
                RaisePropertyChanged();
            }
        }

        public void SetBoolValueProperty(bool value)
        {
	        BoolValueProperty = value;
        }
    }
}