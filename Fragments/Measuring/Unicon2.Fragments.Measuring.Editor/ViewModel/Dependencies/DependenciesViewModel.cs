using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Dependencies;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Common;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies
{
	public class DependenciesViewModel : ViewModelBase
	{
		private readonly ISharedResourcesGlobalViewModel _sharedResourcesGlobalViewModel;
		private IDependencyViewModel _selectedDependency;

		public DependenciesViewModel(ISharedResourcesGlobalViewModel sharedResourcesGlobalViewModel)
		{
			_sharedResourcesGlobalViewModel = sharedResourcesGlobalViewModel;
			AddBoolToAddressDependencyCommand = new RelayCommand(OnAddBoolToAddressDependency);
			RemoveSelectedDependencyCommand =
				new RelayCommand(OnRemoveSelectedDependency, CanExecuteRemoveSelectedDependency);
			SetResourceToSelectedDependencyCommand = new RelayCommand<object>(OnSetResourceToSelectedDependency);
		}

		public void Init(IMeasuringElementEditorViewModel measuringElementEditorViewModel)
		{
			DependencyViewModels = measuringElementEditorViewModel.DependencyViewModels;
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
			DependencyViewModels.Add(new BoolToAddressDependencyViewModel());
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

	}
}