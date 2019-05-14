using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Configuration.ViewModel
{
    public class RuntimeItemGroupViewModel : RuntimeConfigurationItemViewModelBase, IItemGroupViewModel
    {
        private readonly IRuntimeConfigurationItemViewModelFactory _runtimeConfigurationItemViewModelFactory;

        public RuntimeItemGroupViewModel(IRuntimeConfigurationItemViewModelFactory runtimeConfigurationItemViewModelFactory)
        {
            this._runtimeConfigurationItemViewModelFactory = runtimeConfigurationItemViewModelFactory;
            this.IsCheckable = true;
        }

        #region Overrides of ConfigurationItemViewModelBase

        public override string TypeName => ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public override string StrongName => ConfigurationKeys.RUNTIME_DEFAULT_ITEM_GROUP +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        #region Overrides of ConfigurationItemViewModelBase

        protected override void SetModel(object model)
        {
            this.ChildStructItemViewModels.Clear();
            foreach (IConfigurationItem configurationItem in ((IItemsGroup) model).ConfigurationItemList)
            {
                this.ChildStructItemViewModels.Add(this._runtimeConfigurationItemViewModelFactory
                    .CreateRuntimeConfigurationItemViewModel(configurationItem));
            }
            base.SetModel(model);
        }

        #endregion

        #endregion
    }
}