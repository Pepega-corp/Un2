using System;
using System.Windows.Input;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Presentation.Infrastructure.Services
{
	public interface IMainMenuService
	{
		void RegisterMainMenuItem(MainMenuRegistrationOptions menuRegistrationOptions);
	}

	public class MainMenuRegistrationOptions
	{
		public MainMenuRegistrationOptions(Guid itemId, string localizationString, ICommand command, Result<string> groupLocalizationString)
		{
			ItemId = itemId;
			LocalizationString = localizationString;
			Command = command;
			GroupLocalizationString = groupLocalizationString;
		}
		public Guid ItemId { get; }
		public string LocalizationString { get; }
		public ICommand Command { get; }
		public Result<string> GroupLocalizationString { get; }

	}
}