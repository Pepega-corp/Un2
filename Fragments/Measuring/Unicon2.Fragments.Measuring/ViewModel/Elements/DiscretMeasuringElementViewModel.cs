using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public class DiscretMeasuringElementViewModel : MeasuringElementViewModelBase, IDiscretMeasuringElementViewModel
    {

        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;



    }
}