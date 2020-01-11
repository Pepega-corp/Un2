using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Factories;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Factories
{
    public class ConfigurationItemEditorViewModelFactory : IConfigurationItemEditorViewModelFactory
    {
        private readonly ITypesContainer _container;

        public ConfigurationItemEditorViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IEditorConfigurationItemViewModel ResolveConfigurationItemEditorViewModel(IConfigurationItem configurationItem, IEditorConfigurationItemViewModel parent = null)
        {

            IEditorConfigurationItemViewModel configurationItemViewModel =
                this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
            configurationItemViewModel.Model = configurationItem;
            if (parent != null)
            {
                configurationItemViewModel.Parent = parent;
                configurationItemViewModel.Level = parent.Level + 1;
            }

            return configurationItemViewModel;
        }

        public IEditorConfigurationItemViewModel ResolveSubPropertyEditorViewModel(IConfigurationItem configurationItem,
            ObservableCollection<ISharedBitViewModel> mainBitViewModels, IEditorConfigurationItemViewModel parent = null)
        {

            IEditorConfigurationItemViewModel configurationItemViewModel =
                this._container.Resolve<IViewModel>(configurationItem.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL) as IEditorConfigurationItemViewModel;
            (configurationItemViewModel as ISubPropertyEditorViewModel).BitNumbersInWord = mainBitViewModels;
            configurationItemViewModel.Model = configurationItem;
            if (parent != null)
            {
                configurationItemViewModel.Parent = parent;
                configurationItemViewModel.Level = parent.Level + 1;
            }

            return configurationItemViewModel;
        }
    }
}
