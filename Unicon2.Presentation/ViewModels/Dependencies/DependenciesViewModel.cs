using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Dependencies
{
	public class DependenciesViewModel : ViewModelBase
	{
		private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
		private readonly IFormatterEditorFactory _formatterEditorFactory;
		private IDependencyViewModel _selectedDependency;
		private IDependenciesViewModelContainer _dependenciesViewModelContainer;
	    private List<DependencyCreator> _dependencyCreators;

	    public DependenciesViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,IFormatterEditorFactory formatterEditorFactory)
		{
			_sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
			_formatterEditorFactory = formatterEditorFactory;
			RemoveSelectedDependencyCommand =
				new RelayCommand(OnRemoveSelectedDependency, CanExecuteRemoveSelectedDependency);
			SubmitCommand=new RelayCommand<object>(OnSubmit);
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
		    _dependenciesViewModelContainer.DependencyViewModels.Clear();
		    _dependenciesViewModelContainer.DependencyViewModels.AddCollection(DependencyViewModels.CloneCollection());
			window.Close();
		}

	    public void Init(IDependenciesViewModelContainer dependenciesViewModelContainer,
	        DependenciesConfiguration dependenciesConfiguration)
	    {
	        _dependenciesViewModelContainer = dependenciesViewModelContainer;
	        DependencyViewModels =
	            new ObservableCollection<IDependencyViewModel>(_dependenciesViewModelContainer.DependencyViewModels
	                .CloneCollection());
	        DependencyCreators = dependenciesConfiguration.Creators
	            .Select(tuple => new DependencyCreator(this, tuple.dependencyName, tuple.creator)).ToList();
	    }


	    private bool CanExecuteRemoveSelectedDependency()
		{
			return SelectedDependency != null;
		}

		private void OnRemoveSelectedDependency()
		{
			DependencyViewModels.Remove(SelectedDependency);
			SelectedDependency = null;
		}

		

		public IDependencyViewModel SelectedDependency
		{
			get => _selectedDependency;
			set
			{
				_selectedDependency = value;
				RaisePropertyChanged();
				RemoveSelectedDependencyCommand?.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<IDependencyViewModel> DependencyViewModels { get; set; }
		public RelayCommand RemoveSelectedDependencyCommand { get; }

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

    public class DependencyCreator
    {
        public DependencyCreator(DependenciesViewModel dependenciesViewModel,string name,Func<IDependencyViewModel> creator)
        {
            AddDependency = new RelayCommand((() => { dependenciesViewModel.DependencyViewModels.Add(creator()); }));
            Name = name;
        }
        public string Name { get; set; }
        public ICommand AddDependency { get; }
    }
}