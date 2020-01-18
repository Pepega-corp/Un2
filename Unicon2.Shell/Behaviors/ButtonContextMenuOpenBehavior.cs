using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Unicon2.Shell.Behaviors
{
    public class ButtonContextMenuOpenBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Click += AssociatedObject_Click;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= AssociatedObject_Click;
            base.OnDetaching();
        }

        private void AssociatedObject_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AssociatedObject.ContextMenu != null) AssociatedObject.ContextMenu.IsOpen = true;
        }
    }
}