using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels
{
	public class DynamicMainMenuViewModel:ViewModelBase
	{
		public DynamicMainMenuViewModel()
		{
			MenuItems = new ObservableCollection<IMenuItemViewModel>();
		}

		public ObservableCollection<IMenuItemViewModel> MenuItems { get; }
	}

    public interface IMenuItemViewModel: IUniqueId
    {

    }


    public class GroupMenuItemViewModel: IMenuItemViewModel
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

    public class MenuItemViewModel: IMenuItemViewModel
    {
        public MenuItemViewModel( IStronglyNamed stronglyNamedViewModel, Guid id)
        {
            StronglyNamedViewModel = stronglyNamedViewModel;
            Id = id;
        }
        
        public IStronglyNamed StronglyNamedViewModel { get; }

        public Guid Id { get; }
    }

}
