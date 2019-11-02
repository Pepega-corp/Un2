using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;

namespace Unicon2.Fragments.Configuration.Editor.Behaviors
{
    public class TreeGridBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
            AssociatedObject.MouseRightButtonUp += AssociatedObject_MouseRightButtonUp;
            base.OnAttached();
        }


     
        private void AssociatedObject_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem == null) return;

            ((FrameworkElement)e.OriginalSource).ContextMenu = (ContextMenu)AssociatedObject.Resources["ItemContextMenu"]; 
        }

        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as ListView)?.SelectedItem == null) return;

            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;
            if (((ListView) sender).SelectedItem == dataContext)
            {
                ((ConfigurationItemViewModelBase) dataContext).Checked?.Invoke(null);
            }
        }
    }
}