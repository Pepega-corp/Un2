using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Unicon2.SharedResources.Behaviors
{
    public class CollapseableColumnBehavior : Behavior<GridViewColumnHeader>
    {
        public static readonly DependencyProperty CollapseableColumnProperty =
            DependencyProperty.RegisterAttached("CollapseableColumn", typeof(bool), typeof(CollapseableColumnBehavior),
                new UIPropertyMetadata(false, OnCollapseableColumnChanged));

        public static bool GetCollapseableColumn(DependencyObject d)
        {
            return (bool)d.GetValue(CollapseableColumnProperty);
        }

        public static void SetCollapseableColumn(DependencyObject d, bool value)
        {
            d.SetValue(CollapseableColumnProperty, value);
        }

        private static void OnCollapseableColumnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            GridViewColumnHeader header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            header.IsVisibleChanged += AdjustWidth;
        }

        private static void AdjustWidth(object sender, DependencyPropertyChangedEventArgs e)
        {
            GridViewColumnHeader header = sender as GridViewColumnHeader;
            if (header == null)
                return;

            if (header.Visibility == Visibility.Collapsed)
                header.Column.Width = 0;
            else
                header.Column.Width = double.NaN;   // "Auto"
        }
    }
}
