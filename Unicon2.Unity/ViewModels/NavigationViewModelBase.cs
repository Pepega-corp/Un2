using Prism.Regions;
using Unicon2.Unity.Navigation;

namespace Unicon2.Unity.ViewModels
{
    public abstract class NavigationViewModelBase : ViewModelBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return IsNavigationTarget(UniconNavigationContext.FromNavigationContext(navigationContext));
        }

        protected virtual bool IsNavigationTarget(UniconNavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            OnNavigatedFrom(UniconNavigationContext.FromNavigationContext(navigationContext));
  
        }
        protected virtual void OnNavigatedFrom(UniconNavigationContext navigationContext){ }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            OnNavigatedTo(UniconNavigationContext.FromNavigationContext(navigationContext));
        }
        protected virtual void OnNavigatedTo(UniconNavigationContext navigationContext){ }
    }
}
