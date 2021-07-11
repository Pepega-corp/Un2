using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Dependencies
{
    public class AddDependencyViewModel : ViewModelBase
    {
        private List<IDependenciesViewModelContainer> _dependenciesViewModelContainers;
        private List<DependencyCreator> _dependencyCreators;
        private IDependencyViewModel _dependencyToAdd;

        public AddDependencyViewModel()
        {
            SubmitCommand = new RelayCommand<object>(OnSubmit);
            CancelCommand = new RelayCommand<object>(OnCancel);
        }

        private void OnCancel(object obj)
        {
            if (obj is Window window)
            {
                window.Close();
            }
        }

        private void OnSubmit(object obj)
        {
            if (!(obj is Window window)) return;
            _dependenciesViewModelContainers.ForEach(container =>
                container.DependencyViewModels.Add(DependencyToAdd.Clone()));
            window.Close();
        }

        public void Init(List<IDependenciesViewModelContainer> dependenciesViewModelContainers,
            DependenciesConfiguration dependenciesConfiguration)
        {
            _dependenciesViewModelContainers = dependenciesViewModelContainers;

            DependencyCreators = dependenciesConfiguration.Creators
                .Select(tuple => new DependencyCreator(this, tuple.dependencyName, tuple.creator)).ToList();
        }


        public IDependencyViewModel DependencyToAdd
        {
            get => _dependencyToAdd;
            set
            {
                _dependencyToAdd = value; 
                RaisePropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; }

        public ICommand CancelCommand { get; }

        public List<DependencyCreator> DependencyCreators
        {
            get => _dependencyCreators;
            set
            {
                _dependencyCreators = value;
                RaisePropertyChanged();
            }
        }
    }
}