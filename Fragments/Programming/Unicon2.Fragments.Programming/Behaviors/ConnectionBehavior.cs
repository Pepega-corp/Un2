using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using System.Windows.Media;
using System.Windows.Shapes;
using Unicon2.Fragments.Programming.Adorners;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming.Behaviors
{
    // Поведение на нажатие на вывод элемента со связью
    public class ConnectionBehavior : Behavior<Ellipse>
    {
        private const string PATH_NAME = "ConnectionPath"; // должно полностью совпадать с Name у Path в XAML

        private ConnectionViewModel _connectionViewModel;
        private Canvas _designerCanvas;
        private SchemeTabViewModel _tabViewModel;
        private Ellipse _sinkArea;
        private Path _connectionPath;
        private bool _dragEnter;

        public ConnectorViewModel Connector { get; private set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            this._sinkArea = AssociatedObject;
            this._sinkArea.MouseLeftButtonDown += this.SinkAreaOnMouseLeftButtonDown;
            this._sinkArea.MouseMove += this.SinkAreaOnMouseMove;
            this._sinkArea.MouseLeftButtonUp += this.SinkAreaOnMouseLeftButtonUp;

            this.Connector = AssociatedObject.DataContext as ConnectorViewModel;
            this._connectionViewModel = Connector.Connection;

            this._designerCanvas = CommonHelper.GetDesignerCanvas(this._sinkArea);

            this._tabViewModel = this._designerCanvas.DataContext as SchemeTabViewModel;
            
            this._dragEnter = false;

            if (VisualTreeHelper.GetParent(this._sinkArea) is Canvas parent)
            {
                this._connectionPath = parent.Children.OfType<Path>().First(p => p.Name == PATH_NAME);
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

        internal void UpdateConnector(ConnectorViewModel hitConnector)
        {
            Connector = hitConnector;
        }

        private void SinkAreaOnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            if(args.ButtonState == MouseButtonState.Released)
                return;

            foreach (ISelectable item in this._tabViewModel.ElementCollection)
            {
                item.IsSelected = false;
            }

            this._connectionViewModel.IsSelected = true;
            this._dragEnter = true;
            args.Handled = true;
        }
        
        private void SinkAreaOnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            this._dragEnter = false;
        }

        private void SinkAreaOnMouseMove(object sender, MouseEventArgs args)
        {
            if(!this._dragEnter || args.LeftButton != MouseButtonState.Pressed)
                return;

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._designerCanvas);
            if (adornerLayer != null)
            {
                ConnectionAdorner connectorAdorner = new ConnectionAdorner(this._designerCanvas, this);
                adornerLayer.Add(connectorAdorner);
                args.Handled = true;
            }
        }

        private void OnPreviewLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
            {
                this._connectionViewModel.IsSelected = !this._connectionViewModel.IsSelected;
            }
            else if (!this._connectionViewModel.IsSelected)
            {
                foreach (ISelectable item in this._tabViewModel.ElementCollection)
                {
                    item.IsSelected = false;
                }

                this._connectionViewModel.IsSelected = true;
            }
        }
    }
}
