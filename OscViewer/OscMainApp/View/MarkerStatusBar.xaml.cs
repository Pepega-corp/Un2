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
using Oscilloscope.View.MainItem;

namespace Oscilloscope.View
{
    /// <summary>
    /// Логика взаимодействия для MarkerStatusBar.xaml
    /// </summary>
    public partial class MarkerStatusBar : UserControl
    {
        public MarkerStatusBar()
        {
            InitializeComponent();
            Binding binding = BindingOperations.GetBinding(Marker1Tb, TextBox.TextProperty);
            MyRule = new IsIntValidationRule();
            binding.ValidationRules.Add(MyRule);
        }

        public IsIntValidationRule MyRule { get; set; }


        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set
            {
              //  MyRule.Max = value;
                SetValue(MaximumProperty, value);
            }
        }
        public static readonly DependencyProperty MaximumProperty =
      DependencyProperty.Register("Maximum", typeof(int), typeof(MarkerStatusBar), new PropertyMetadata(0));


        private int _marker1;
        public int Marker1
        {
            get { return _marker1; }
            set
            {
                _marker1 = value;
                Marker1Tb.Text = _marker1.ToString(CultureInfo.CurrentCulture);
                this.CalcDelta();
            }
        }


        private int _marker2;
        public int Marker2
        {
            get { return _marker2; }
            set
            {
                _marker2 = value;
                Marker2Tb.Text = _marker2.ToString(CultureInfo.CurrentCulture);
                this.CalcDelta();
            }
        }

        private void CalcDelta()
        {
            DeltaTb.Text = string.Format("{0} мс", Math.Abs(_marker1 - _marker2).ToString(CultureInfo.CurrentCulture));
        }

        public event Action<int> MoveMarker1;
        public event Action<int> MoveMarker2;
        private void Marker1Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Validation.GetHasError(Marker1Tb))
            {
                return;
            }

            if (e.Key == Key.Return)
            {
                try
                {
                    var res = (int)uint.Parse(Marker1Tb.Text);
                    if (res>Maximum)
                    {
                        res = Maximum;
                    }
                    Marker1 = res;
                    if (MoveMarker1 != null)
                    {
                        MoveMarker1.Invoke(Marker1);
                    }
                }
                catch
                {
                    Marker1Tb.Text = Marker1.ToString(CultureInfo.InvariantCulture);
                }
            } 
        }

        private void Marker2Tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (Validation.GetHasError(Marker2Tb))
            {
                return;
            }
            if (e.Key == Key.Return)
            {
                try
                {
                    var res = (int)uint.Parse(Marker2Tb.Text);
                    if (res > Maximum)
                    {
                        res = Maximum;
                    }
                    Marker2 = res;
                    if (MoveMarker2 != null)
                    {
                        MoveMarker2.Invoke(Marker2);
                    }
                }
                catch
                {
                    Marker2Tb.Text = Marker2.ToString(CultureInfo.InvariantCulture);
                }
               
            }
        }

        private void Marker1Tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private bool IsTextAllowed(string text)
        {
            try
            {
                int.Parse(text);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
