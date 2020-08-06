using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Dependencies;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies
{
	public class DependenciesViewModel : ViewModelBase
	{
		private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
		private readonly IFormatterEditorFactory _formatterEditorFactory;
		private IDependencyViewModel _selectedDependency;
		private IMeasuringElementEditorViewModel _measuringElementEditorViewModel;

		public DependenciesViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel,IFormatterEditorFactory formatterEditorFactory)
		{
			_sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
			_formatterEditorFactory = formatterEditorFactory;
			AddBoolToAddressDependencyCommand = new RelayCommand(OnAddBoolToAddressDependency);
			RemoveSelectedDependencyCommand =
				new RelayCommand(OnRemoveSelectedDependency, CanExecuteRemoveSelectedDependency);
			SetResourceToSelectedDependencyCommand = new RelayCommand<object>(OnSetResourceToSelectedDependency);
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
			_measuringElementEditorViewModel.DependencyViewModels.Clear();
			_measuringElementEditorViewModel.DependencyViewModels.AddCollection(DependencyViewModels.CloneCollection());
			window.Close();
		}

		public void Init(IMeasuringElementEditorViewModel measuringElementEditorViewModel)
		{
			_measuringElementEditorViewModel = measuringElementEditorViewModel;
			DependencyViewModels = new ObservableCollection<IDependencyViewModel>(measuringElementEditorViewModel.DependencyViewModels.CloneCollection());
		}

		private void OnSetResourceToSelectedDependency(object o)
		{
			((BoolToAddressDependencyViewModel) o).RelatedResourceName =
				_sharedResourcesGlobalViewModel
					.OpenSharedResourcesForSelectingString<IDiscretMeasuringElementEditorViewModel>();
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

		private void OnAddBoolToAddressDependency()
		{
			DependencyViewModels.Add(new BoolToAddressDependencyViewModel(_formatterEditorFactory));
		}

		public IDependencyViewModel SelectedDependency
		{
			get => _selectedDependency;
			set
			{
				_selectedDependency = value;
				RaisePropertyChanged();
				RemoveSelectedDependencyCommand?.RaiseCanExecuteChanged();
				SetResourceToSelectedDependencyCommand?.RaiseCanExecuteChanged();
			}
		}

		public ObservableCollection<IDependencyViewModel> DependencyViewModels { get; set; }
		public ICommand AddBoolToAddressDependencyCommand { get; }
		public RelayCommand RemoveSelectedDependencyCommand { get; }
		public RelayCommand<object> SetResourceToSelectedDependencyCommand { get; }

		public ICommand SubmitCommand { get; }

		public ICommand CancelCommand { get; }

	}
}