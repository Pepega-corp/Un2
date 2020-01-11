using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeDependentPropertyViewModel : RuntimePropertyViewModel
    {


        public RuntimeDependentPropertyViewModel(ITypesContainer container, IValueViewModelFactory valueViewModelFactory) : base(container, valueViewModelFactory)
        {
        }

        public override string StrongName => ConfigurationKeys.RUNTIME + ConfigurationKeys.DEPENDENT_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        protected override string GetTypeName()
        {
            return ConfigurationKeys.DEPENDENT_PROPERTY;
        }

        protected override void InitializeValueViewModel(ushort[] ushorts, bool isLocal, IUshortsFormatter ushortsFormatter)
        {
            base.InitializeValueViewModel(ushorts, isLocal,
                isLocal
                    ? ((IDependentProperty) this._model).LocalValueUshortsFormatter
                    : ((IDependentProperty) this._model).DeviceValueUshortsFormatter);
        }
    }
}
