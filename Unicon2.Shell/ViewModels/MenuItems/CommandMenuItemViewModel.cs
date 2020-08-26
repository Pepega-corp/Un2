using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels.MenuItems
{
	public class CommandMenuItemViewModel : ViewModelBase, IStronglyNamed
	{
		public CommandMenuItemViewModel(ICommand command, string nameKey)
		{
			Command = command;
			NameKey = nameKey;
		}

		public ICommand Command { get; }
		public string NameKey { get; }
		public string StrongName => ApplicationGlobalNames.UiCommandStrings.COMMAND;
	}
}