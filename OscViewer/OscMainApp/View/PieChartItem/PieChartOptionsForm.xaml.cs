using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Oscilloscope.View.PieChartItem.Characteristics;
using DataGridCell = System.Windows.Controls.DataGridCell;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Oscilloscope.View.PieChartItem
{
    /// <summary>
    /// Логика взаимодействия для PieChartOptionsForm.xaml
    /// </summary>
    public partial class PieChartOptionsForm : Window
    {
        private const int MIN_START_TIME = 20;

        public int Marker1 { get; set; }
        public int Marker2 { get; set; }
        private PieChartOptions _chartOptions;

        public PieChartOptions ChartOptions
        {
            get { return _chartOptions; }
            set
            {
                if (_chartOptions != null)
                {
                    ChartOptions.ChannelChanged -= ChannelsChanged;
                }
           
                _chartOptions = value;
                Characteristics = _chartOptions.Characteristics;
                Open();
                this.Prepare();
            }
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell == null)
            {
                return;
            }
            if (!cell.IsEditing)
            {
                // enables editing on single click
                if (!cell.IsFocused)
                    cell.Focus();
                if (!cell.IsSelected)
                    cell.IsSelected = true;
            }
        }

        public PieChartOptionsForm()
        {
            InitializeComponent();

        
            PolyGrid.DataContext = _polyCharacteristicOption;
            RoundGrid.DataContext = _roundCharacteristicOption;
            DirectionGrid.DataContext = _directionCharacteristicOption;
            ChargeGrid.DataContext = _chargeCharacteristicOption;
        }

        private void Prepare()
        {
            IOptions.ItemsSource = ChartOptions.PieIChartOptions;
            UOptions.ItemsSource = ChartOptions.PieUChartOptions;

            ChartOptions.ChannelChanged += ChannelsChanged;

            StartTimeTb.Text = ChartOptions.StartTime.ToString(CultureInfo.CurrentCulture);
            EndTimeTb.Text = ChartOptions.EndTime.ToString(CultureInfo.CurrentCulture);
            ChannelsChanged();
        }

  



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.ReadData())
            {
               this.DialogResult = true; 
            }
        }


        private bool ReadData()
        {
            int startTime;
            try
            {
                startTime = int.Parse(StartTimeTb.Text);
                if ((startTime < MIN_START_TIME) || (startTime > ChartOptions.Lenght-1))
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format("Значение начального времени должно быть целым числом от {0} до {1}",
                                              MIN_START_TIME, ChartOptions.Lenght-1));
                return false;
            }

            int endTime;
            try
            {
                endTime = int.Parse(EndTimeTb.Text);
                if ((endTime <= startTime) || (endTime > ChartOptions.Lenght-1))
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(
                    string.Format(
                        "Значение конечного времени должно быть целым числом большим чем начальное время и меньшим {0}",
                        ChartOptions.Lenght-1));
                return false;
            }

            ZnOptions z1;
            try
            {
                z1 = Z1Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N1");
                return false;
            }


            ZnOptions z2;
            try
            {
                z2 = Z2Editor.Zn;

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N2");
                return false;
            }


            ZnOptions z3;
            try
            {
                z3 = Z3Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N3");
                return false;
            }


            ZnOptions z4;
            try
            {
                z4 = Z4Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N4");
                return false;
            }


            ZnOptions z5;
            try
            {
                z5 = Z5Editor.Zn;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка ввода коэффициентов компенсации по контуру Ф-N5");
                return false;
            }

            ChartOptions.Z1 = z1;
            ChartOptions.Z2 = z2;
            ChartOptions.Z3 = z3;
            ChartOptions.Z4 = z4;
            ChartOptions.Z5 = z5;
            ChartOptions.StartTime = startTime;
            ChartOptions.EndTime = endTime;
            ChartOptions.Characteristics = Characteristics;
            ChartOptions.ChannelAccept();
            return true;
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        private void ChannelsChanged()
        {
            if (ChartOptions == null)
            {
                return;
            }

            bool ia = ChartOptions.PieIChartOptions.Any(o => o.A);
            bool ib = ChartOptions.PieIChartOptions.Any(o => o.B);
            bool ic = ChartOptions.PieIChartOptions.Any(o => o.C);
            bool i0 = ia & ib & ic;


            bool ua = ChartOptions.PieUChartOptions.Any(o => o.A);
            bool ub = ChartOptions.PieUChartOptions.Any(o => o.B);
            bool uc = ChartOptions.PieUChartOptions.Any(o => o.C);

            bool z1 = Z1Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z2 =Z2Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z3 = Z3Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z4 = Z4Editor.Stage1Cb.IsChecked.GetValueOrDefault();
            bool z5 = Z5Editor.Stage1Cb.IsChecked.GetValueOrDefault();

            List<VisibilityItem> res = new List<VisibilityItem>();


            if (ia & ib & ua & ub) res.Add(ChartOptions.VisiblyOptions.Zab);
            if (ic & ib & uc & ub) res.Add(ChartOptions.VisiblyOptions.Zbc);
            if (ic & ia & uc & ua) res.Add(ChartOptions.VisiblyOptions.Zca);

            if (ia & i0 & ua & z1) res.Add(ChartOptions.VisiblyOptions.Z1A);
            if (ib & i0 & ub & z1) res.Add(ChartOptions.VisiblyOptions.Z1B);
            if (ic & i0 & uc & z1) res.Add(ChartOptions.VisiblyOptions.Z1C);

            if (ia & i0 & ua & z2) res.Add(ChartOptions.VisiblyOptions.Z2A);
            if (ib & i0 & ub & z2) res.Add(ChartOptions.VisiblyOptions.Z2B);
            if (ic & i0 & uc & z2) res.Add(ChartOptions.VisiblyOptions.Z2C);

            if (ia & i0 & ua & z3) res.Add(ChartOptions.VisiblyOptions.Z3A);
            if (ib & i0 & ub & z3) res.Add(ChartOptions.VisiblyOptions.Z3B);
            if (ic & i0 & uc & z3) res.Add(ChartOptions.VisiblyOptions.Z3C);

            if (ia & i0 & ua & z4) res.Add(ChartOptions.VisiblyOptions.Z4A);
            if (ib & i0 & ub & z4) res.Add(ChartOptions.VisiblyOptions.Z4B);
            if (ic & i0 & uc & z4) res.Add(ChartOptions.VisiblyOptions.Z4C);

            if (ia & i0 & ua & z5) res.Add(ChartOptions.VisiblyOptions.Z5A);
            if (ib & i0 & ub & z5) res.Add(ChartOptions.VisiblyOptions.Z5B);
            if (ic & i0 & uc & z5) res.Add(ChartOptions.VisiblyOptions.Z5C);
            VisiblyOptionsGrid.ItemsSource = res;
        }

        List<ICharacteristic> _allCharacteristic = new List<ICharacteristic>();

        private PieChartPolyCharacteristicOption _polyCharacteristicOption = new PieChartPolyCharacteristicOption();
        private PieChartRoundCharacteristicOption _roundCharacteristicOption = new PieChartRoundCharacteristicOption();
        private PieChartChargeCharacteristicOption _chargeCharacteristicOption = new PieChartChargeCharacteristicOption();
        private PieChartDirectionCharacteristicOption _directionCharacteristicOption = new PieChartDirectionCharacteristicOption();
        private List<ICharacteristic> Characteristics
        {
            get { return _allCharacteristic; }
            set
            {
                listBox.Items.Clear();
                if (value != null)
                {
                    _allCharacteristic = value;
                    foreach (var characteristic in _allCharacteristic)
                    {
                        listBox.Items.Add(characteristic);
                    }
                }
            }

        }

  

        private void PolyAddButton_Click(object sender, RoutedEventArgs e)
        {
             _polyCharacteristicOption = new PieChartPolyCharacteristicOption();
             _polyCharacteristicOption.F = double.Parse(PolyFTb.Text, CultureInfo.CurrentCulture);
             _polyCharacteristicOption.R = double.Parse(PolyRTb.Text, CultureInfo.CurrentCulture);
             _polyCharacteristicOption.X = double.Parse(PolyXTb.Text, CultureInfo.CurrentCulture);
            _polyCharacteristicOption.Enabled = true;
            _allCharacteristic.Add(_polyCharacteristicOption);
            listBox.Items.Add(_polyCharacteristicOption);
        }

        private void RoundAddButton_Click(object sender, RoutedEventArgs e)
        {
             _roundCharacteristicOption = new PieChartRoundCharacteristicOption();
             _roundCharacteristicOption.Radius = double.Parse(RoundRadiusTb.Text, CultureInfo.CurrentCulture);
             _roundCharacteristicOption.R = double.Parse(RoundRTb.Text, CultureInfo.CurrentCulture);
             _roundCharacteristicOption.X = double.Parse(RoundXTb.Text, CultureInfo.CurrentCulture);
            _roundCharacteristicOption.Enabled = true;
            _allCharacteristic.Add(_roundCharacteristicOption);
            listBox.Items.Add(_roundCharacteristicOption);
        }

        private void ChargeAddButton_Click(object sender, RoutedEventArgs e)
        {
             _chargeCharacteristicOption = new PieChartChargeCharacteristicOption();
             _chargeCharacteristicOption.R1 = double.Parse(ChargeR1Tb.Text, CultureInfo.CurrentCulture);
             _chargeCharacteristicOption.R2 = double.Parse(ChargeR2Tb.Text, CultureInfo.CurrentCulture);
             _chargeCharacteristicOption.F = double.Parse(ChargeFTb.Text, CultureInfo.CurrentCulture);
            _chargeCharacteristicOption.Enabled = true;
            _allCharacteristic.Add(_chargeCharacteristicOption);
            listBox.Items.Add(_chargeCharacteristicOption);
        }

        private void DirectionAddButton_Click(object sender, RoutedEventArgs e)
        {
             _directionCharacteristicOption = new PieChartDirectionCharacteristicOption();
             _directionCharacteristicOption.F1 = double.Parse(DirectionF1Tb.Text, CultureInfo.CurrentCulture);
             _directionCharacteristicOption.F2 = double.Parse(DirectionF2Tb.Text, CultureInfo.CurrentCulture);

            _directionCharacteristicOption.Enabled = true;
            _allCharacteristic.Add(_directionCharacteristicOption);
            listBox.Items.Add(_directionCharacteristicOption);
        }

      

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                _allCharacteristic.Remove((ICharacteristic)listBox.SelectedItem);
                listBox.Items.Remove(listBox.SelectedItem);
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ReadData())
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Файл параметров круговой диаграммы|*.pco";
                if (saveDialog.ShowDialog().GetValueOrDefault())
                {
                    this._chartOptions.Save(saveDialog.FileName);
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файл параметров круговой диаграммы|*.pco";
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                try
                {
                    this._chartOptions.Load(openFileDialog.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка загрузки файла");
                    return;
                }

                this.Characteristics = this._chartOptions.Characteristics;
                this.Open();
            }
        }


        private void Open()
        {
            Z1Editor.Zn = this._chartOptions.Z1;
            Z2Editor.Zn = this._chartOptions.Z2;
            Z3Editor.Zn = this._chartOptions.Z3;
            Z4Editor.Zn = this._chartOptions.Z4;
            Z5Editor.Zn = this._chartOptions.Z5;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            StartTimeTb.Text = Math.Min(Marker1, Marker2).ToString(CultureInfo.CurrentCulture);
            EndTimeTb.Text = Math.Max(Marker1, Marker2).ToString(CultureInfo.CurrentCulture);
        }


        private void ColorLabelDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var label = (Label) sender;
            var oldColor =  ((SolidColorBrush)label.Background).Color;
      
            ColorDialog dialog = new ColorDialog();


            dialog.Color = System.Drawing.Color.FromArgb(oldColor.A, oldColor.R, oldColor.G, oldColor.B);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var visibilityItem = (VisibilityItem) VisiblyOptionsGrid.SelectedItem;
                  visibilityItem.LineColor = new SolidColorBrush(  Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
              
             // label.GetBindingExpression(Label.BackgroundProperty).UpdateTarget();
            }
        }

      
    }



}
