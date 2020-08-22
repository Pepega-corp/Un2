using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels.MenuItems
{
	public class ToggleOptionsMenuItemViewModel:ViewModelBase,IStronglyNamed
	{
	    private readonly ShellViewModel _shellViewModel;

	    public ToggleOptionsMenuItemViewModel(ShellViewModel shellViewModel)
	    {
	        _shellViewModel = shellViewModel;
	    }

        

	    public bool IsOpen
	    {
	        get => _shellViewModel.IsMenuFlyOutOpen;
	        set => _shellViewModel.IsMenuFlyOutOpen = value;
	    }

	    public string StrongName => nameof(ToggleOptionsMenuItemViewModel);

	    public void Update()
	    {
            RaisePropertyChanged(nameof(IsOpen));
	    }
	}
}
