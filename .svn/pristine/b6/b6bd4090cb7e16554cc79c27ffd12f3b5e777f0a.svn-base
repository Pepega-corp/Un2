using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Fluent;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using WPFLocalizeExtension.Engine;

namespace Unicon2.Shell.Behaviors
{
    public class DynamicRibbonTabItemBehavior : Behavior<RibbonTabItem>
    {
        public static readonly DependencyProperty TabItemsProperty = DependencyProperty.Register("TabItems", typeof(IFragmentOptionsViewModel), typeof(DynamicRibbonTabItemBehavior), new PropertyMetadata(null, OnTabItemsPropertyChanged));

        public IFragmentOptionsViewModel TabItems
        {
            get { return (IFragmentOptionsViewModel)GetValue(TabItemsProperty); }
            set { SetValue(TabItemsProperty, value); }
        }

        private static void OnTabItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var beh = sender as DynamicRibbonTabItemBehavior;
            beh.OnTabItemsChanged();
        }

        protected virtual void OnTabItemsChanged()
        {
            AssociatedObject.Groups.Clear();

            if (TabItems == null) return;

            List<RibbonGroupBox> ribbonGroupBoxs = new List<RibbonGroupBox>();
            foreach (IFragmentOptionGroupViewModel fragmentOptionGroupViewModel in TabItems.FragmentOptionGroupViewModels)
            {
                RibbonGroupBox ribbonGroupBox = new RibbonGroupBox();
                ribbonGroupBox.Header = LocalizeDictionary.Instance
                    .GetLocalizedObject(fragmentOptionGroupViewModel.NameKey, null, LocalizeDictionary.Instance.Culture).ToString();

                foreach (IFragmentOptionCommandViewModel fragmentOptionCommandViewModel in fragmentOptionGroupViewModel.FragmentOptionCommandViewModels)
                {
                    string header = LocalizeDictionary.Instance
                        .GetLocalizedObject(fragmentOptionCommandViewModel.TitleKey, null,
                            LocalizeDictionary.Instance.Culture).ToString();
                    if (fragmentOptionCommandViewModel is IFragmentOptionToggleCommandViewModel)
                    {
                        Fluent.CheckBox toggleButton = new CheckBox();
                        toggleButton.Header = header;
                        toggleButton.SetBinding(Fluent.CheckBox.IsCheckedProperty,
                            new Binding("IsChecked")
                            {
                                Source = fragmentOptionCommandViewModel,
                                Mode = BindingMode.TwoWay
                            });
                        toggleButton.Size = RibbonControlSize.Middle;
                        ribbonGroupBox.Items.Add(toggleButton);

                    }
                    else
                    {
                        Fluent.Button button = new Fluent.Button();
                        button.Size = RibbonControlSize.Middle;
                        button.Command = fragmentOptionCommandViewModel.OptionCommand;
                        button.Header = header;
                        ribbonGroupBox.Items.Add(button);
                    }
                }
                ribbonGroupBoxs.Add(ribbonGroupBox);
            }

            AssociatedObject.Groups.AddRange(ribbonGroupBoxs);
        }


        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
        }

        #endregion
    }
}
