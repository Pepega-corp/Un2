using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
	public class DynamicMainMenuViewModel : ViewModelBase
	{
		public DynamicMainMenuViewModel()
		{
			MenuItems = new ObservableCollection<IMenuItemViewModel>();
		}

		public ObservableCollection<IMenuItemViewModel> MenuItems { get; }
	}

	public interface IMenuItemViewModel : IUniqueId
	{

	}


	public class GroupMenuItemViewModel : IMenuItemViewModel
	{
		public GroupMenuItemViewModel(string groupNameKey, Guid id)
		{
			GroupNameKey = groupNameKey;
			Id = id;
			ChildMenuItems = new ObservableCollection<IMenuItemViewModel>();
		}

		public string GroupNameKey { get; }
		public ObservableCollection<IMenuItemViewModel> ChildMenuItems { get; }

		public Guid Id { get; }
	}

	public class MenuItemViewModel : IMenuItemViewModel
	{
		public MenuItemViewModel(IStronglyNamed stronglyNamedViewModel, Guid id)
		{
			StronglyNamedViewModel = stronglyNamedViewModel;
			Id = id;
		}

		public IStronglyNamed StronglyNamedViewModel { get; }

		public Guid Id { get; }
	}

	public class MenuItemCommandViewModel : IMenuItemViewModel
	{
		public MenuItemCommandViewModel(ICommand command, string nameKey, Guid id)
		{
			Command = command;
			NameKey = nameKey;
			Id = id;
		}


		public ICommand Command { get; }
		public string NameKey { get; }
		public Guid Id { get; }
	}

}