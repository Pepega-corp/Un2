using System;
using System.Globalization;
using System.Windows;

namespace Oscilloscope.View.MainItem
{
    /// <summary>
    /// Логика взаимодействия для CatForm.xaml
    /// </summary>
    public partial class CutForm : Window
    {
        private int _start;
        private int _end;
        private string _oscName;

        public CutForm()
        {
            InitializeComponent();
        }

        public int Start
        {
            get { return _start; }
            set
            {
                _start = value;
                StartTb.Text = _start.ToString(CultureInfo.InvariantCulture);
            }
        }

        public int End
        {
            get { return _end; }
            set
            {
                _end = value;
                EndTb.Text = _end.ToString(CultureInfo.InvariantCulture);
            }
        }


        public string OscName
        {
            get { return _oscName; }
            set
            {
                _oscName = value;
                NameTb.Text = value;
            }
        }
        public int Min { get; set; }
        public int Max { get; set; }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Start = int.Parse(StartTb.Text);
                End = int.Parse(EndTb.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Проверте правильность ввода данных");
                return;
            }
            if (Start>= End)
            {
                MessageBox.Show("Значение начала больше или равно значению конца");
                return;
            }
            if (End > Max)
            {
                MessageBox.Show("Значение конца больше допустимого");
                return;
            }
            if (Start < Min)
            {
                MessageBox.Show("Значение начала меньше допустимого");
                return;
            }
            OscName = NameTb.Text;
            this.DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        public int Marker1 { get; set; }
        public int Marker2 { get; set; }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StartTb.Text = Math.Min(Marker1, Marker2).ToString(CultureInfo.InvariantCulture);
            EndTb.Text = Math.Max(Marker1, Marker2).ToString(CultureInfo.InvariantCulture);
        }
    }
}
