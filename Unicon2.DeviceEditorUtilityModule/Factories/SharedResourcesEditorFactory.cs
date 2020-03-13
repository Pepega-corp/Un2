using System;
using System.Windows;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Factories
{
    public class SharedResourcesEditorFactory : ISharedResourcesEditorFactory
    {
        private readonly ITypesContainer _container;

        public SharedResourcesEditorFactory(ITypesContainer container)
        {
            _container = container;
        }

        public void OpenResourceForEdit(IResourceViewModel resource, object _owner)
        {
            IApplicationGlobalCommands applicationGlobalCommands = _container.Resolve<IApplicationGlobalCommands>();
            if (applicationGlobalCommands != null)
            {
                IResourceEditingViewModel resourceEditingViewModel = _container.Resolve<IResourceEditingViewModel>();

                if (resource.RelatedEditorItemViewModel is IFormatterParametersViewModel
                    formatterParametersViewModel)
                {
                    resourceEditingViewModel.ResourceEditorViewModel = formatterParametersViewModel.RelatedUshortsFormatterViewModel;
                    applicationGlobalCommands.ShowWindowModal(() => new ResourcesEditingWindow(), resourceEditingViewModel, _owner);
                }


            
            }
        }
    }
}
