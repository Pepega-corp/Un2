using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Adorners;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Behaviors
{
    // Поведение на нажатие на вывод элемента со связью
    public class ConnectionBehavior : Behavior<Ellipse>
    {
        private ConnectionViewModel _connection;
        private Canvas _designerCanvas;
        private SchemeTabViewModel _tabViewModel;
        private Ellipse _sinkArea;
        private Path _connectionPath;
        private bool _dragEnter;

        protected override void OnAttached()
        {
            base.OnAttached();
            this._sinkArea = AssociatedObject;
            this._sinkArea.MouseLeftButtonDown += this.SinkAreaOnMouseLeftButtonDown;
            this._sinkArea.MouseMove += this.SinkAreaOnMouseMove;
            this._sinkArea.MouseLeftButtonUp += this.SinkAreaOnMouseLeftButtonUp;
            this._connection = AssociatedObject.DataContext as ConnectionViewModel;
            this._designerCanvas = CommonHelper.GetDesignerCanvas(this._sinkArea);
            this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
            
            this._dragEnter = false;

            Canvas parent = VisualTreeHelper.GetParent(this._sinkArea) as Canvas;
            if (parent != null)
            {
                this._connectionPath = parent.Children.OfType<Path>().First(p => p.Name == ConnectionViewModel.PATH_NAME);
                if (this._connectionPath != null)
                {
                    this._connectionPath.PreviewMouseLeftButtonDown += this.OnPreviewLeftMouseDown;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this._sinkArea.MouseLeftButtonDown -= this.SinkAreaOnMouseLeftButtonDown;
            this._sinkArea.MouseMove -= this.SinkAreaOnMouseMove;
            this._sinkArea.MouseLeftButtonUp -= this.SinkAreaOnMouseLeftButtonUp;
            this._connectionPath.PreviewMouseLeftButtonDown -= this.OnPreviewLeftMouseDown;
        }

        private void SinkAreaOnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            if(args.ButtonState == MouseButtonState.Released) return;
            foreach (ISelectable item in this._tabViewModel.ElementCollection)
                item.IsSelected = false;
            this._connection.IsSelected = true;
            this._dragEnter = true;
            args.Handled = true;
        }
        
        private void SinkAreaOnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            this._dragEnter = false;
        }

        private void SinkAreaOnMouseMove(object sender, MouseEventArgs args)
        {
            if(!this._dragEnter || args.LeftButton != MouseButtonState.Pressed) return;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
            {
                ConnectionAdorner connectorAdorner = new ConnectionAdorner(this._designerCanvas, this._connection);
                adornerLayer.Add(connectorAdorner);
                args.Handled = true;
            }
        }

        private void OnPreviewLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
            {
                this._connection.IsSelected = !this._connection.IsSelected;
            }
            else if (!this._connection.IsSelected)
            {
                foreach (ISelectable item in this._tabViewModel.ElementCollection)
                    item.IsSelected = false;
                this._connection.IsSelected = true;
            }
        }
    }
}
