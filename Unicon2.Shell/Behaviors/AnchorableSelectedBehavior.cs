using System;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Unicon2.Presentation.Infrastructure.ViewModels.Windows;
using Xceed.Wpf.AvalonDock.Layout;

namespace Unicon2.Shell.Behaviors
{
    public class AnchorableSelectedBehavior : Behavior<LayoutAnchorable>
    {
        #region Overrides of Behavior

        protected override void OnAttached()
        {
            AssociatedObject.IsSelectedChanged += AssociatedObject_IsSelectedChanged;
            base.OnAttached();
        }

        private void AssociatedObject_IsSelectedChanged(object sender, EventArgs e)
        {
            if ((sender as LayoutAnchorable).Content is ContentControl)
            {
                if (((sender as LayoutAnchorable).Content as ContentControl).Content is ILogServiceViewModel)
                {
                    (((sender as LayoutAnchorable).Content as ContentControl).Content as ILogServiceViewModel)
                        .RefreshHeaderStringCommand.Execute(null);
                }
            }
        }

        #endregion
    }
}
