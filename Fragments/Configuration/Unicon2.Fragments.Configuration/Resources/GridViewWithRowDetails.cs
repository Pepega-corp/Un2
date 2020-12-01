using System.Windows;
using System.Windows.Controls;

namespace Unicon2.Fragments.Configuration.Resources
{
    public class GridViewWithRowDetails : GridView
    {
        public static DataTemplate GetRowDetailsTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(RowDetailsTemplateProperty);
        }

        public static void SetRowDetailsTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(RowDetailsTemplateProperty, value);
        }

        public static readonly DependencyProperty RowDetailsTemplateProperty =
            DependencyProperty.RegisterAttached("RowDetailsTemplate", typeof(DataTemplate), typeof(GridViewWithRowDetails), new UIPropertyMetadata());


        protected override void PrepareItem(ListViewItem item)
        {
            base.PrepareItem(item);
            item.SetValue(RowDetailsTemplateProperty, GetValue(RowDetailsTemplateProperty));

        }

        protected override object ItemContainerDefaultStyleKey
        {
            get
            {
                return new ComponentResourceKey(GetType(), "ItemContainerStyleKey");
            }
        }
    }
}
