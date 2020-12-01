using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
	public class AddFragmentViewModel : ViewModelBase
	{
		private readonly ITypesContainer _container = StaticContainer.Container;
		private IFragmentEditorViewModel _selectedFragment;
		private IResultingDeviceViewModel _resultingDeviceViewModel;
		private string _fragmentName;

		public AddFragmentViewModel(IResultingDeviceViewModel resultingDeviceViewModel)
		{
			_resultingDeviceViewModel = resultingDeviceViewModel;
			AvailableFragments=new ObservableCollection<IFragmentEditorViewModel>(); 
			IEnumerable<IFragmentEditorViewModel> fragments = _container.ResolveAll<IFragmentEditorViewModel>();
			foreach (IFragmentEditorViewModel fragment in fragments)
			{
				AvailableFragments.Add(fragment);
			}
			AddSelectedFragmentCommand=new RelayCommand<object>(OnAddSelectedFragment, CanAddSelectedFragment);
		}

		private bool CanAddSelectedFragment(object obj)
		{
			if (SelectedFragment == null)
			{
				return false;
			}
			if (!(SelectedFragment is INameable))
			{
				if (_resultingDeviceViewModel.FragmentEditorViewModels.Any((model =>
					model.NameForUiKey == SelectedFragment.NameForUiKey)))
				{
					return false;
				}
			}
			return SelectedFragment != null;
		}

		private void OnAddSelectedFragment(object obj)
		{
			
			if (!(SelectedFragment is INameable))
			{
				_resultingDeviceViewModel.FragmentEditorViewModels.Add(SelectedFragment);
				_resultingDeviceViewModel.SelectedFragmentEditorViewModel = SelectedFragment;
			}
			else
			{
				var newOne = this._container.Resolve<IFragmentEditorViewModel>(SelectedFragment.StrongName);
				_resultingDeviceViewModel.FragmentEditorViewModels.Add(newOne);
				(newOne as INameable).Name = FragmentName;
				_resultingDeviceViewModel.SelectedFragmentEditorViewModel = newOne;
			}

			if (obj is Window window)
			{
				window.Close();
			}
		}

		public IFragmentEditorViewModel SelectedFragment
		{
			get => _selectedFragment;
			set
			{
				_selectedFragment = value; 
				RaisePropertyChanged();
				AddSelectedFragmentCommand?.RaiseCanExecuteChanged();
			}
		}

		public string FragmentName
		{
			get => _fragmentName;
			set
			{
				_fragmentName = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<IFragmentEditorViewModel> AvailableFragments { get; }
		public RelayCommand<object> AddSelectedFragmentCommand { get; }
	}
}