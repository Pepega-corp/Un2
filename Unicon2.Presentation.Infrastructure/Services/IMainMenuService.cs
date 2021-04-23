using System;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IMainMenuService
    {
        void RegisterMainMenuItem(MainMenuRegistrationOptions menuRegistrationOptions);
        void RegisterMainMenuItemGroup(MainMenuGroupRegistrationOptions menuRegistrationOptions);
        void RegisterMainMenuCommand(MainMenuCommandRegistrationOptions mainMenuCommandRegistrationOptions);

    }

    public class MainMenuGroupRegistrationOptions
    {
        public MainMenuGroupRegistrationOptions(Guid itemId, string groupLocalizationString, int priority = 100,
            string mainMenuGroupId = null)
        {
            ItemId = itemId;
            GroupLocalizationString = groupLocalizationString;
            Priority = priority;
            MainMenuGroupId = mainMenuGroupId;
        }

        public Guid ItemId { get; }
        public string GroupLocalizationString { get; }
        public int Priority { get; }
        public string MainMenuGroupId { get; }


    }
    public class MainMenuCommandRegistrationOptions
    {
        public MainMenuCommandRegistrationOptions(Guid itemId, ICommand command, string itemNameKey, int priority = 100,
            string mainMenuGroupId = null)
        {
            ItemId = itemId;
            Command = command;
            ItemNameKey = itemNameKey;
            Priority = priority;
            MainMenuGroupId = mainMenuGroupId;
        }

        public Guid ItemId { get; }
        public ICommand Command { get; }
        public string ItemNameKey { get; }
        public int Priority { get; }
        public string MainMenuGroupId { get; }

    }
    public class MainMenuRegistrationOptions
    {
        public MainMenuRegistrationOptions(Guid itemId, IStronglyNamed viewModelStronglyNamed, int proirity = 100,
            string mainMenuGroupId = null)
        {
            ItemId = itemId;
            ViewModelStronglyNamed = viewModelStronglyNamed;
            Proirity = proirity;
            MainMenuGroupId = mainMenuGroupId;
        }

        public Guid ItemId { get; }
        public IStronglyNamed ViewModelStronglyNamed { get; }
        public int Proirity { get; }
        public string MainMenuGroupId { get; }

    }
}