using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public class DiscretMeasuringElementViewModel : MeasuringElementViewModelBase, IDiscretMeasuringElementViewModel
    {
        private bool _isReadingEnabled;
        private bool _isWritingEnabled;
        private IDataProvider _dataProvider;

        public DiscretMeasuringElementViewModel(IValueViewModelFactory valueViewModelFactory, IBoolValue boolValue)
        {
            this.FormattedValueViewModel = valueViewModelFactory.CreateFormattedValueViewModel(boolValue);
        }

        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        protected override void SetModel(object model)
        {
            base.SetModel(model);
            this._measuringElement.ElementChangedAction += () =>
            {
                (this.FormattedValueViewModel as IBoolValueViewModel).BoolValueProperty = (this._measuringElement as IDiscretMeasuringElement).DeviceValue;
            };

        }
    }
}
