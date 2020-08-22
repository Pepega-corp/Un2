using System;
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
			throw new NotImplementedException();
		}
	}
}
