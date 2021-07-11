using System;
using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;

namespace Unicon2.Presentation.Infrastructure.Services.Dependencies
{
    public interface IDependenciesService
    {
        void EditDependencies(IDependenciesViewModelContainer dependenciesViewModelContainer,DependenciesConfiguration dependenciesConfiguration);

        void AddDependencyToManyProps(List<IDependenciesViewModelContainer> containers,
            DependenciesConfiguration dependenciesConfiguration);
    }

    public class DependenciesConfiguration
    {
        public List<(string dependencyName, Func<IDependencyViewModel> creator)> Creators=new List<(string dependencyName, Func<IDependencyViewModel> creator)>();
        public DependenciesConfiguration(params (string dependencyName, Func<IDependencyViewModel> creator)[] creators)
        {
            Creators.AddRange(creators);
        }
    }
   
}