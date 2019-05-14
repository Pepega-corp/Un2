using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class MarkerControlChart : UserControl
    {
        private int _lenght ;
        public event Action<int> Move;
        public int Lenght
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                Marker.Maximum = _lenght;

            }
        }

        public int Value
        {
            get { return (int) Marker.Marker1; }
            set
            {
                Marker.Marker1 = value;
                this.TimeBox.Text = Marker.Marker1.ToString();
            }
        }
        public MarkerControlChart()
        {
            InitializeComponent();
        }

      

        private void RaiseMove(int index)
        {
            if (Move!= null)
            {
                Move.Invoke(index);
            }
        }
        public void Scroll(double offset)
        {
            this.TimeBox.Margin = new Thickness(offset, 0, 0, 0);
        }

     

    

        private void TimeBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    var time = uint.Parse(TimeBox.Text);
                    if (time> this.Lenght)
                    {
                        MessageBox.Show("Значение превышает допустимый диапазон");
                    }
                    this.Value = (int) time;
                }
                catch (Exception)
                {

                    MessageBox.Show("Значение содержит неверные символы");
                }
            }
            
        }

        private void MoveMarker()
        {

            this.TimeBox.Text = Marker.Marker1.ToString();
            this.RaiseMove(Marker.Marker1);
        }
    }
}
