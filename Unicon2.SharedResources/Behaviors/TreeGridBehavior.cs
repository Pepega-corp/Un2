using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.SharedResources.Behaviors
{
    public class TreeGridBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
           
            AssociatedObject.MouseDoubleClick += this.AssociatedObject_MouseDoubleClick;
        
            base.OnAttached();
        }

        private void AssociatedObject_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem == null) return;

            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;
            if((sender as ListView).SelectedItem== dataContext)
                ((sender as ListView).SelectedItem as IConfigurationItemViewModel).Checked?.Invoke(null);
        }
    }
}