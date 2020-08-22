using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
	public class DynamicMainMenuViewModel:ViewModelBase
	{
		public DynamicMainMenuViewModel()
		{
			MenuItems = new ObservableCollection<MainMenuItemViewModel>();
		}

		public ObservableCollection<MainMenuItemViewModel> MenuItems { get; }
	}

	public class MainMenuItemViewModel
	{
		public MainMenuItemViewModel(bool isGrouped)
		{
			MenuItems = new ObservableCollection<MainMenuItemViewModel>();
			IsGrouped = isGrouped;
		}

		public bool IsGrouped { get; }
		public ObservableCollection<MainMenuItemViewModel> MenuItems { get; }

	}
}
