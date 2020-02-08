using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Factories
{
    public class RuntimeConfigurationItemViewModelFactory : IRuntimeConfigurationItemViewModelFactory
    {
        private readonly ITypesContainer _container;

        public RuntimeConfigurationItemViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IRuntimeConfigurationItemViewModel CreateRuntimeConfigurationItemViewModel(
            IConfigurationItem configurationItem)
        {
            IRuntimeConfigurationItemViewModel configurationItemViewModel =
                this._container.Resolve<IConfigurationItemViewModel>(ConfigurationKeys.RUNTIME +
                                                                     configurationItem.StrongName +
                                                                     ApplicationGlobalNames.CommonInjectionStrings
                                                                         .VIEW_MODEL) as
                    IRuntimeConfigurationItemViewModel;
            configurationItemViewModel.Model = configurationItem;
            return configurationItemViewModel;
        }

        public IConfigurationItemViewModel CreateGroupWithReiterationViewModel(IReiterationSubGroupInfo reiterationSubGroupInfo)
        {
            GroupWithReiterationViewModel groupWithReiterationViewModel = new GroupWithReiterationViewModel();
            groupWithReiterationViewModel.Header = reiterationSubGroupInfo.Name;
            groupWithReiterationViewModel.ChildStructItemViewModels =
                new ObservableCollection<IConfigurationItemViewModel>(
                    reiterationSubGroupInfo.ConfigurationItems.Select(CreateRuntimeConfigurationItemViewModel));

            return groupWithReiterationViewModel;
        }
    }
}
