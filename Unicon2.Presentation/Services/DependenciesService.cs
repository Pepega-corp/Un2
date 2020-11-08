using System;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Presentation.ViewModels.Dependencies;
using Unicon2.Presentation.Views;

namespace Unicon2.Presentation.Services
{
    public class DependenciesService : IDependenciesService
    {
        private readonly Func<DependenciesViewModel> _depVmFunc;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;

        public DependenciesService(Func<DependenciesViewModel> depVmFunc,IApplicationGlobalCommands applicationGlobalCommands)
        {
            _depVmFunc = depVmFunc;
            _applicationGlobalCommands = applicationGlobalCommands;
        }

        public void EditDependencies(IDependenciesViewModelContainer dependenciesViewModelContainer,
            DependenciesConfiguration dependenciesConfiguration)
        {
            var viewModel = _depVmFunc();
            viewModel.Init(dependenciesViewModelContainer,dependenciesConfiguration);
            _applicationGlobalCommands.ShowWindowModal(() => new DependenciesView(), viewModel);
        }
    }
}