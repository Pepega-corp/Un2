using MahApps.Metro.Controls;
using Unicon2.Infrastructure.Common;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.Views
{
	/// <summary>
	/// Логика взаимодействия для DynamicMainMenu.xaml
	/// </summary>
	public partial class DynamicMainMenu : WindowCommands
	{
		public DynamicMainMenu()
		{
			InitializeComponent();
			DataContext = StaticContainer.Container.Resolve<DynamicMainMenuViewModel>();
		}
	}
}