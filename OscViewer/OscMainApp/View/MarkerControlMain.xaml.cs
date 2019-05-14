using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Oscilloscope.View
{
    /// <summary>
    /// Логика взаимодействия для MarkerControl.xaml
    /// </summary>
    public partial class MarkerControlMain : UserControl
    {
        private int _lenght ;
        public event Action<int> Move;
        public event Action<int> MoveSecond;
        private View.Zoom _zoom;
        public int Lenght
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                Markers.Maximum = _lenght;
               // Markers.Width = _lenght + 10;
               // this.Width = _lenght + 155;
            }
        }

        public Zoom Zoom
        {
            set
            {
               // Markers.Width = _lenght * value.ZoomX + 10;
                //this.Width = _lenght * value.ZoomX + 155;
            }
        }

        public int Value
        {
            get { return Markers.Marker1; }
            set { Markers.Marker1 = value; }
        }

        public MarkerControlMain()
        {
            InitializeComponent();
        }

        private void Marker_ValueChanged()
        {
            this.RaiseMove(Markers.Marker1);
        }

        private void Marker2_ValueChanged()
        {
            if (MoveSecond != null)
            {
                MoveSecond.Invoke(Markers.Marker2);
            }
        }

        private void RaiseMove(int index)
        {
            if (Move!= null)
            {
                Move.Invoke(index);
            }
        }
        public void Scroll(ScrollChangedEventArgs e)
        {

        }

        internal void SetZoom(Zoom zoom)
        {
            this._zoom = zoom;
            Markers.SetZoom(_zoom);
        }
    }
}
