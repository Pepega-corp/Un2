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
        
        public bool IsGroupedProperty
        {
            get { return this._isGroupedProperty; }
            set
            {
                this._isGroupedProperty = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
