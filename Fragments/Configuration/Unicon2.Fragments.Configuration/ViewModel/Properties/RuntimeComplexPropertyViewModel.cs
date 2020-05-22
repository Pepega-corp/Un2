using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeComplexPropertyViewModel : RuntimePropertyViewModel, IRuntimeComplexPropertyViewModel
    {
        private bool _isGroupedProperty;
        
        protected override string GetTypeName()
        {
            return ConfigurationKeys.COMPLEX_PROPERTY;
        }

        public override string StrongName => ConfigurationKeys.RUNTIME+ConfigurationKeys.COMPLEX_PROPERTY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public bool IsGroupedProperty
        {
            get { return _isGroupedProperty; }
            set
            {
                _isGroupedProperty = value;
                RaisePropertyChanged();
            }
        }
    }
}
