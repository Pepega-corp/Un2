using System;
using System.Collections.Generic;
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
        private readonly Func<AddDependencyViewModel> _addDepVmFunc;
        private readonly IApplicationGlobalCommands _applicationGlobalCommands;

        public DependenciesService(Func<DependenciesViewModel> depVmFunc, Func<AddDependencyViewModel> addDepVmFunc,IApplicationGlobalCommands applicationGlobalCommands)
        {
            _depVmFunc = depVmFunc;
            _addDepVmFunc = addDepVmFunc;
            _applicationGlobalCommands = applicationGlobalCommands;
        }

        public void EditDependencies(IDependenciesViewModelContainer dependenciesViewModelContainer,
            DependenciesConfiguration dependenciesConfiguration)
        {
            var viewModel = _depVmFunc();
            viewModel.Init(dependenciesViewModelContainer,dependenciesConfiguration);
            _applicationGlobalCommands.ShowWindowModal(() => new DependenciesView(), viewModel);
        }

        public void AddDependencyToManyProps(List<IDependenciesViewModelContainer> containers, DependenciesConfiguration dependenciesConfiguration)
        {
            var viewModel = _addDepVmFunc();
            viewModel.Init(containers,dependenciesConfiguration);
            _applicationGlobalCommands.ShowWindowModal(()=>new AddDependencyView(), viewModel);
        }
    }
}