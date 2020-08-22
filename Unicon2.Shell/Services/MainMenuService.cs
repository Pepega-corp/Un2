﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.Services
{
	public class MainMenuService:IMainMenuService
	{
		private readonly DynamicMainMenuViewModel _dynamicMainMenuViewModel;

		public MainMenuService(DynamicMainMenuViewModel dynamicMainMenuViewModel)
		{
			_dynamicMainMenuViewModel = dynamicMainMenuViewModel;
		}

	    public void RegisterMainMenuItem(MainMenuRegistrationOptions menuRegistrationOptions)
	    {
	        if (menuRegistrationOptions.MainMenuGroupId == null)
	        {
	            if (_dynamicMainMenuViewModel.MenuItems.All(model => model.Id != menuRegistrationOptions.ItemId))
	            {
	                _dynamicMainMenuViewModel.MenuItems.Add(
	                    new MenuItemViewModel(menuRegistrationOptions.ViewModelStronglyNamed,
	                        menuRegistrationOptions.ItemId));
	            }

	        }
	        else
	        {
	            var group = _dynamicMainMenuViewModel.MenuItems.FirstOrDefault(model =>
	                model is GroupMenuItemViewModel groupMenuItemViewModel && groupMenuItemViewModel.GroupNameKey ==
	                menuRegistrationOptions.MainMenuGroupId) as GroupMenuItemViewModel;
                   group.ChildMenuItems.Add(new MenuItemViewModel(menuRegistrationOptions.ViewModelStronglyNamed,
                       menuRegistrationOptions.ItemId));
            }

	    }

	    public void RegisterMainMenuItemGroup(MainMenuGroupRegistrationOptions menuRegistrationOptions)
	    {
	        if (menuRegistrationOptions.MainMenuGroupId == null)
	        {
	            if (_dynamicMainMenuViewModel.MenuItems.All(model => model.Id != menuRegistrationOptions.ItemId))
	            {
	                _dynamicMainMenuViewModel.MenuItems.Add(
	                    new GroupMenuItemViewModel(menuRegistrationOptions.GroupLocalizationString,
	                        menuRegistrationOptions.ItemId));
	            }

	        }

        }
    }
}
