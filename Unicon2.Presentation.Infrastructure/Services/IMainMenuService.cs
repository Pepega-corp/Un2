using System;
using System.Windows.Input;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.Services
{
    public interface IMainMenuService
    {
        void RegisterMainMenuItem(MainMenuRegistrationOptions menuRegistrationOptions);
        void RegisterMainMenuItemGroup(MainMenuGroupRegistrationOptions menuRegistrationOptions);

    }

    public class MainMenuGroupRegistrationOptions
    {
        public MainMenuGroupRegistrationOptions(Guid itemId, string groupLocalizationString, int proirity = 100,
            string mainMenuGroupId = null)
        {
            ItemId = itemId;
            GroupLocalizationString = groupLocalizationString;
            Proirity = proirity;
            MainMenuGroupId = mainMenuGroupId;
        }

        public Guid ItemId { get; }
        public string GroupLocalizationString { get; }
        public int Proirity { get; }
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