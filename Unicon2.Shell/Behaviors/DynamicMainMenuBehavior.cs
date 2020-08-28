using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using MahApps.Metro.Controls;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.DataTemplateSelectors;
using Unicon2.Shell.ViewModels;

namespace Unicon2.Shell.Behaviors
{
    public class DynamicMainMenuBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register("MenuItems", typeof(ObservableCollection<IMenuItemViewModel>), 
            typeof(DynamicMainMenuBehavior), new PropertyMetadata(null, OnMenuItemsPropertyChanged));

        private static void OnMenuItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var behavior = (d as DynamicMainMenuBehavior);
            var assosiatedObject = behavior.AssociatedObject;
            if (assosiatedObject == null) return;
            var menuItems = behavior.MenuItems;
            assosiatedObject.ContextMenu = new ContextMenu();

            foreach (var menuItem in menuItems)
            {
                if (menuItem is MenuItemViewModel menuItemViewModel)
                {
                 
                    var template=new ViewModelByStrongNameDataTemplateSelector().SelectTemplate(menuItemViewModel.StronglyNamedViewModel,assosiatedObject);
                    var kox=template.LoadContent().GetChildObjects().First();
                    if (kox is MenuItem menuItemTemplated)
                    {
                        assosiatedObject.ContextMenu.Items.Add(menuItemTemplated);
                    }
                }
                else if (menuItem is MenuItemCommandViewModel menuItemCommandViewModel)
                {
                    assosiatedObject.ContextMenu.Items.Add(new MenuItem()
                    {
                        Header = StaticContainer.Container.Resolve<ILocalizerService>()
                            .GetLocalizedString(menuItemCommandViewModel.NameKey),
                        Command = menuItemCommandViewModel.Command
                    });
                }
            }
        }


        protected override void OnAttached()
        {
            OnMenuItemsPropertyChanged(this, new DependencyPropertyChangedEventArgs());
            base.OnAttached();
        }

        public ObservableCollection<IMenuItemViewModel> MenuItems
        {
            get { return (ObservableCollection<IMenuItemViewModel>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

    }
}