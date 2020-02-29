using System;
using System.Windows;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.DeviceEditorUtilityModule.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Interfaces;

namespace Unicon2.DeviceEditorUtilityModule.Factories
{
    public class SharedResourcesEditorFactory : ISharedResourcesEditorFactory
    {
        private readonly ITypesContainer _container;

        public SharedResourcesEditorFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public void OpenResourceForEdit(INameable resource, object _owner)
        {
            IStronglyNamed stronglyNamed = resource as IStronglyNamed;

            if (stronglyNamed == null) return;

            IApplicationGlobalCommands applicationGlobalCommands = this._container.Resolve<IApplicationGlobalCommands>();
            if (applicationGlobalCommands != null)
            {
                IResourceEditingViewModel resourceEditingViewModel = this._container.Resolve<IResourceEditingViewModel>();
                IViewModel resViewModel;
                try
                {
                    resViewModel = this._container.Resolve<IViewModel>(stronglyNamed.StrongName +
                        ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                }
                catch (Exception)
                {
                    return;
                }
                if (resViewModel == null) return;
                resViewModel.Model = resource;
                resourceEditingViewModel.ResourceEditorViewModel = resViewModel;
                applicationGlobalCommands.ShowWindowModal(() => new ResourcesEditingWindow(), resourceEditingViewModel, _owner);
            }
        }
    }
}
