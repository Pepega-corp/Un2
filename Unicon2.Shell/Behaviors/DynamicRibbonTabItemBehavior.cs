using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Shell.Views;
using WPFLocalizeExtension.Engine;

namespace Unicon2.Shell.Behaviors
{
    public class DynamicMenuTabItemBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TabItemsProperty = DependencyProperty.Register("TabItems", typeof(IFragmentOptionsViewModel), typeof(DynamicMenuTabItemBehavior), new PropertyMetadata(null, OnTabItemsPropertyChanged));

        public IFragmentOptionsViewModel TabItems
        {
            get { return (IFragmentOptionsViewModel)GetValue(TabItemsProperty); }
            set { SetValue(TabItemsProperty, value); }
        }

        private static void OnTabItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var beh = sender as DynamicMenuTabItemBehavior;
            beh.OnTabItemsChanged();
        }

        protected virtual void OnTabItemsChanged()
        {
            AssociatedObject.ContextMenu = new ContextMenu();            

            if (TabItems == null) return;

            List<object> menuItems = new List<object>();
            foreach (IFragmentOptionGroupViewModel fragmentOptionGroupViewModel in TabItems.FragmentOptionGroupViewModels)
            {
                string headerGroup = LocalizeDictionary.Instance
                     .GetLocalizedObject(fragmentOptionGroupViewModel.NameKey, null,
                     LocalizeDictionary.Instance.Culture).ToString();
             //   menuItems.Add(new TextBlock() { Text = headerGroup });
                var visTree = new FrameworkElementFactory(typeof(StackPanel));
                var textBlockVisTree = new FrameworkElementFactory(typeof(TextBlock));
                textBlockVisTree.SetValue(TextBlock.TextProperty, headerGroup);
                visTree.AppendChild(textBlockVisTree);
                visTree.AppendChild(new FrameworkElementFactory(typeof(Separator)));
                menuItems.Add(new Separator() { Template = new ControlTemplate() { VisualTree = visTree } });

                foreach (IFragmentOptionCommandViewModel fragmentOptionCommandViewModel in fragmentOptionGroupViewModel.FragmentOptionCommandViewModels)
                {
                    string header = LocalizeDictionary.Instance
                    .GetLocalizedObject(fragmentOptionCommandViewModel.TitleKey, null,
                    LocalizeDictionary.Instance.Culture).ToString();
                    if (fragmentOptionCommandViewModel is IFragmentOptionToggleCommandViewModel)
                    {
                        CheckBox toggleButton = new CheckBox();
                        toggleButton.Content = header;
                        toggleButton.SetBinding(CheckBox.IsCheckedProperty,
                        new Binding("IsChecked")
                        {
                            Source = fragmentOptionCommandViewModel,
                            Mode = BindingMode.TwoWay
                        });
                        menuItems.Add(toggleButton);
                    }
                    else
                    {
                        MenuItem button = new MenuItem();
                        button.Command = fragmentOptionCommandViewModel.OptionCommand;
                        button.Header = header;
                        menuItems.Add(button);
                    }
                }
               
            }
            menuItems.ForEach((item => AssociatedObject.ContextMenu.Items.Add(item)));
        }


        protected override void OnAttached()
        {
            base.OnAttached();
        }
    }
}