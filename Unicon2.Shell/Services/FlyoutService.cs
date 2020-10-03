using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.Services
{
    public class FlyoutService:IFlyoutService
    {
        private List<IFlyoutProvider> _flyoutProviders;

        public FlyoutService()
        {
            _flyoutProviders=new List<IFlyoutProvider>();
        }
   
        public void CloseFlyout()
        {
            _flyoutProviders.ForEach(provider => provider.IsFlyoutOpen = false);
        }

        public void RegisterFlyout(IFlyoutProvider flyoutProvider)
        {
            if (!_flyoutProviders.Contains(flyoutProvider))
            {
                _flyoutProviders.Add(flyoutProvider);
            }
        }

        public void UnregisterFlyout(IFlyoutProvider flyoutProvider)
        {
            if (_flyoutProviders.Contains(flyoutProvider))
            {
                _flyoutProviders.Remove(flyoutProvider);
            }
        }

     
    }
}
