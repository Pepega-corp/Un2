using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeSubPropertyViewModel : RuntimePropertyViewModel
    {
        public RuntimeSubPropertyViewModel(ITypesContainer container, IValueViewModelFactory valueViewModelFactory) : base(container, valueViewModelFactory)
        {
        }


        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.SUB_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        protected override string GetTypeName()
        {
            return ConfigurationKeys.SUB_PROPERTY;
        }
    }
}
