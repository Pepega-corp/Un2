using CommonServiceLocator;
using Prism.Regions;
using System;

namespace Unicon2.Unity.Navigation
{
    public class UniconNavigationContext
    {
        public Uri Uri { get; set; }
        public UniconNavigationParameters NavigationParameters { get; set; }

        public NavigationContext ToNavigationContext()
        {
            NavigationContext result = new NavigationContext(ServiceLocator.Current.GetInstance<IRegionNavigationService>(), Uri, null);
            NavigationParameters?.ForEach(parameter =>
            {
                result.Parameters.Add(parameter.ParameterName, parameter.Parameter);
            });
            return result;
        }

        public static UniconNavigationContext FromNavigationContext(NavigationContext navigationContext)
        {
            UniconNavigationContext uniconNavigationContext = new UniconNavigationContext();
            if (navigationContext == null)
            {
                return null;
            }
            uniconNavigationContext.NavigationParameters = UniconNavigationParameters.FromNavigationParameters(navigationContext.Parameters);
            uniconNavigationContext.Uri = navigationContext.Uri;
            return uniconNavigationContext;
        }
    }
}
