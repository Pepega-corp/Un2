using System.Collections.Generic;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Factories
{
    public class SharedResourcesEditorFactory : ISharedResourcesEditorFactory
    {
        private readonly ITypesContainer _container;
        private readonly IFormatterViewModelFactory _formatterViewModelFactory;

        public SharedResourcesEditorFactory(ITypesContainer container,
            IFormatterViewModelFactory formatterViewModelFactory)
        {
            _container = container;
            _formatterViewModelFactory = formatterViewModelFactory;
        }

        public void OpenResourceForEdit(IResourceViewModel resource, object _owner)
        {
            IApplicationGlobalCommands applicationGlobalCommands = _container.Resolve<IApplicationGlobalCommands>();
            if (applicationGlobalCommands != null)
            {
                IResourceEditingViewModel resourceEditingViewModel = _container.Resolve<IResourceEditingViewModel>();

                if (resource.RelatedEditorItemViewModel is IUshortsFormatter
                    formatter)
                {
                    //todo

                }



            }
        }
    }
}
