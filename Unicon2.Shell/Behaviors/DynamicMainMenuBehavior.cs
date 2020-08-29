using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.DataTemplateSelectors;
using Unicon2.Shell.ViewModels;
using WPFLocalizeExtension.Providers;

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
                    var content=template.LoadContent().GetChildObjects().First();

                    if (content is MenuItem menuItemTemplated)
                    {
                        DisconnectIt(menuItemTemplated);
                        menuItemTemplated.DataContext = menuItemViewModel.StronglyNamedViewModel;
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
        public static void DisconnectIt(FrameworkElement child)
        {
            var parent = child.Parent;
            if (parent == null)
                return;

            if (parent is Panel panel)
            {
                panel.Children.Remove(child);
                return;
            }

            if (parent is Decorator decorator)
            {
                if (decorator.Child == child)
                    decorator.Child = null;

                return;
            }

            if (parent is ContentPresenter contentPresenter)
            {
                if (contentPresenter.Content == child)
                    contentPresenter.Content = null;
                return;
            }

            if (parent is ContentControl contentControl)
            {
                if (contentControl.Content == child)
                    contentControl.Content = null;
                return;
            }

            //if (parent is ItemsControl itemsControl)
            //{
            //  itemsControl.Items.Remove(child);
            //  return;
            //}
        }


    }
}