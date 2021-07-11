using System;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Commands;

namespace Unicon2.Presentation.ViewModels.Dependencies
{
    public class DependencyCreator
    {
        public DependencyCreator(DependenciesViewModel dependenciesViewModel, string name,
            Func<IDependencyViewModel> creator)
        {
            AddDependency = new RelayCommand((() => { dependenciesViewModel.DependencyViewModels.Add(creator()); }));
            Name = name;
        }

        public DependencyCreator(AddDependencyViewModel dependenciesViewModel, string name,
            Func<IDependencyViewModel> creator)
        {
            AddDependency = new RelayCommand(() => { dependenciesViewModel.DependencyToAdd = creator(); });
            Name = name;
        }

        public string Name { get; set; }
        public ICommand AddDependency { get; }
    }
}