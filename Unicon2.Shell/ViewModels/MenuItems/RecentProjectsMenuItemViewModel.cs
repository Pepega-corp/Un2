using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Shell.ViewModels.MenuItems
{
    public class RecentProjectsMenuItemViewModel : ViewModelBase, IStronglyNamed
    {
        private List<RecentProjectViewModel> _recentProjects;

        public RecentProjectsMenuItemViewModel(ICommand openRecentProjectCommand)
        {
            OpenRecentProjectCommand = openRecentProjectCommand;
        }

        public ICommand OpenRecentProjectCommand { get; }

        public List<RecentProjectViewModel> RecentProjects
        {
            get => _recentProjects;
            set
            {
                _recentProjects = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName => nameof(RecentProjectsMenuItemViewModel);

        
    }
}