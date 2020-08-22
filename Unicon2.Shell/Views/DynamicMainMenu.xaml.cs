using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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