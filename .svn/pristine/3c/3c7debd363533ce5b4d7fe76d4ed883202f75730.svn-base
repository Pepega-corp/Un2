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
            get { return (TextBlock)this.GetValue(FucusableTextBlockProperty); }
            set { this.SetValue(FucusableTextBlockProperty, value); }
        }

        private static void OnFucusableTextBlockPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FocusableInformationBehavior beh = sender as FocusableInformationBehavior;
            beh.OnFucusableTextBlockChanged();
        }

        private void OnFucusableTextBlockChanged()
        {
            
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            this.FucusableTextBlock.MouseEnter += (ea, se) =>
            {
                this.AssociatedObject.Visibility = Visibility.Visible;
            };
            this.FucusableTextBlock.MouseLeave += (ea, se) =>
            {
                this.AssociatedObject.Visibility = Visibility.Hidden;
            };
            base.OnAttached();
        }

        #endregion
    }
}
