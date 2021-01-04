using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Configuration.ViewModel.Table;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.DataTemplateSelectors
{
    public class FilteredViewModelDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IStronglyNamed)
            {

                FrameworkElement element = container as FrameworkElement;


                var relatedWrapper = (((container as ContentPresenter).TemplatedParent as ContentControl).DataContext as
                    List<ILocalAndDeviceValueContainingViewModel>).First(model => model.LocalValue == item);

                if (relatedWrapper is ConfigItemWrapper configItemWrapper)
                {
                    if (!configItemWrapper.MatchesFilter)
                    {
                        //var x =  element?.TryFindResource(((IStronglyNamed)item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;

                        return element?.TryFindResource(ApplicationGlobalNames.CommonInjectionStrings.FILTERED +
                                                        ((IStronglyNamed) item).StrongName +
                                                        ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as
                            DataTemplate;
                    }
                }

                //var x =  element?.TryFindResource(((IStronglyNamed)item).StrongName + ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as DataTemplate;

                return element?.TryFindResource(((IStronglyNamed) item).StrongName +
                                                ApplicationGlobalNames.CommonInjectionStrings.DATATEMPLATE) as
                    DataTemplate;
            }

            return null;
        }
    }
}