using Prism.Regions;
using Unicon2.Unity.Navigation;

namespace Unicon2.Unity.ViewModels
{
    public abstract class NavigationViewModelBase : ViewModelBase, INavigationAware
    {
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return this.IsNavigationTarget(UniconNavigationContext.FromNavigationContext(navigationContext));
        }

        protected virtual bool IsNavigationTarget(UniconNavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            this.OnNavigatedFrom(UniconNavigationContext.FromNavigationContext(navigationContext));
  
        }
        protected virtual void OnNavigatedFrom(UniconNavigationContext navigationContext){ }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            this.OnNavigatedTo(UniconNavigationContext.FromNavigationContext(navigationContext));
        }
        protected virtual void OnNavigatedTo(UniconNavigationContext navigationContext){ }
    }
}
