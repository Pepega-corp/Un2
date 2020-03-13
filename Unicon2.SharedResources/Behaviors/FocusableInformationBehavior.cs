using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Shapes;

namespace Unicon2.SharedResources.Behaviors
{
    public class FocusableInformationBehavior : Behavior<Rectangle>
    {
        public static readonly DependencyProperty FucusableTextBlockProperty =
            DependencyProperty.Register("FucusableTextBlock", typeof(TextBlock), typeof(FocusableInformationBehavior), new PropertyMetadata(null, OnFucusableTextBlockPropertyChanged));

        public TextBlock FucusableTextBlock
        {
            get { return (TextBlock)GetValue(FucusableTextBlockProperty); }
            set { SetValue(FucusableTextBlockProperty, value); }
        }

        private static void OnFucusableTextBlockPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FocusableInformationBehavior beh = sender as FocusableInformationBehavior;
            beh.OnFucusableTextBlockChanged();
        }

        private void OnFucusableTextBlockChanged()
        {
            
        }

        protected override void OnAttached()
        {
            FucusableTextBlock.MouseEnter += (ea, se) =>
            {
                AssociatedObject.Visibility = Visibility.Visible;
            };
            FucusableTextBlock.MouseLeave += (ea, se) =>
            {
                AssociatedObject.Visibility = Visibility.Hidden;
            };
            base.OnAttached();
        }
    }
}
