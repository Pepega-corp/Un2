using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Oscilloscope.ComtradeFormat;
using Oscilloscope.Properties;
using Oscilloscope.View.AnalogChartItem;
using Xceed.Wpf.AvalonDock.Layout;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace Oscilloscope.View.MainItem
{
    public class ColorCollection
    {
        private Color[] _colors;

        public ColorCollection()
        {
            this._colors = new Color[]
                {
                    (Color)ColorConverter.ConvertFromString("#ffe2c247"),
                    (Color)ColorConverter.ConvertFromString("#ffc38c28"),
                    (Color)ColorConverter.ConvertFromString("#ffb92d16"),
                    (Color)ColorConverter.ConvertFromString("#ffff3205"),
                    (Color)ColorConverter.ConvertFromString("#ff9f3d22"),
                    (Color)ColorConverter.ConvertFromString("#ff22672e"),
                    (Color)ColorConverter.ConvertFromString("#ff50502e"),
                    (Color)ColorConverter.ConvertFromString("#ff382449"),
                    (Color)ColorConverter.ConvertFromString("#ff24474e"),
                    (Color)ColorConverter.ConvertFromString("#ff2f3986"),
                    (Color)ColorConverter.ConvertFromString("#ff248683"),
                    (Color)ColorConverter.ConvertFromString("#ff81b2b1"),
                    (Color)ColorConverter.ConvertFromString("#ff7a851c"),
                    (Color)ColorConverter.ConvertFromString("#ffac6343"),
                    (Color)ColorConverter.ConvertFromString("#ffb18055"),
                    (Color)ColorConverter.ConvertFromString("#ff03e074"),
                    (Color)ColorConverter.ConvertFromString("#ff92278f"),
                    (Color)ColorConverter.ConvertFromString("#ff0072bc"),
     
                };
        }

        public Color[] Colors
        {
            get { return this._colors; }
            set
            {
                if (value == null)
                {
                    return;
                }
                this._colors = value;
            }
        }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        const int SW_SHOWNORMAL = 1;
        const int SW_SHOWMINIMIZED = 2;
        private int _encodding;

        static MainWindow()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
        }
        public static ColorCollection PieChartColors = new ColorCollection();

        public static Brush Marker1Brush = new SolidColorBrush(Color.FromRgb(0,0,90));
        public static Brush Marker2Brush = new SolidColorBrush(Color.FromRgb(130, 0, 0));

        private static void MyHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception) e.ExceptionObject;
            MessageBox.Show(ex.Message);
            MessageBox.Show(ex.StackTrace);
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Width = SystemParameters.WorkArea.Width*0.8;
            Height = SystemParameters.WorkArea.Height*0.8;
            Application.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            try
            {
                WINDOWPLACEMENT wp = Settings.Default.WindowPlacement;
                wp.length = Marshal.SizeOf(typeof (WINDOWPLACEMENT));
                wp.flags = 0;
                wp.showCmd = (wp.showCmd == SW_SHOWMINIMIZED ? SW_SHOWNORMAL : wp.showCmd);
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                SetWindowPlacement(hwnd, ref wp);
                this.FrequencyCheckBox.IsChecked = Settings.Default.Frequency;
                this.PieCheckBox.IsChecked = Settings.Default.Pie;
                this.VectorCheckBox.IsChecked = Settings.Default.Vector;
                this._openInitialDirectory = Settings.Default.OpenDirectory;
                this._saveInitialDirectory = Settings.Default.SaveDirectory;
                this.FrequencyPane.DockHeight = Settings.Default.FrequencyHeight;
                this.RightPanel.DockWidth = Settings.Default.RightWidth;
                this.VectorPane.DockHeight = Settings.Default.VectorHeight;

                this.MinCheckBox.IsChecked = Settings.Default.AnalogMin;
                this.MaxCheckBox.IsChecked = Settings.Default.AnalogMax;
                this.RmsCheckBox.IsChecked = Settings.Default.AnalogRms;
                this.FirstFarmocCheckBox.IsChecked = Settings.Default.AnalogFirstHarmonic;

                CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State = (CommonDiscretChartItems.CommonDiscretChart.States) Settings.Default.DiscretVisibleOption;
                if (Settings.Default.PieChartOptions != null)
                {
                    this.CurrentWorkplace.CurrentPieChartOptions.VisiblyOptions = Settings.Default.PieChartOptions;
                }

                this.ApplyDiscretVisibly();
            }
            catch(Exception es)
            {
            }
        }


        private void ApplyDiscretVisibly()
        {
            switch (CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State)
            {
                case CommonDiscretChartItems.CommonDiscretChart.States.ALL:
                {
                    this.DiscretAllButton.IsChecked = true;
                    break;
                }
                case CommonDiscretChartItems.CommonDiscretChart.States.ENABLED:
                {
                    this.DiscretEnabledButton.IsChecked = true;
                    break;
                }
                case CommonDiscretChartItems.CommonDiscretChart.States.HIDE:
                {
                    this.DiscretDisabledButton.IsChecked = true;
                    break;
                }
            }
        }

        // WARNING - Not fired when Application.SessionEnding is fired
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                base.OnClosing(e);
                // Persist window placement details to application settings
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                GetWindowPlacement(hwnd, out wp);
                Settings.Default.WindowPlacement = wp;
                Settings.Default.Frequency = this.FrequencyCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.Pie = this.PieCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.Vector = this.VectorCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.OpenDirectory = this._openInitialDirectory;
                Settings.Default.SaveDirectory = this._saveInitialDirectory;
                Settings.Default.FrequencyHeight = this.FrequencyPane.DockHeight;
                Settings.Default.RightWidth = this.RightPanel.DockWidth;
                Settings.Default.VectorHeight = this.VectorPane.DockHeight;
                Settings.Default.DiscretVisibleOption = (int) CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State;

                Settings.Default.AnalogMin = this.MinCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.AnalogMax = this.MaxCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.AnalogRms = this.RmsCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.AnalogFirstHarmonic = this.FirstFarmocCheckBox.IsChecked.GetValueOrDefault();
                Settings.Default.PieChartOptions = this.CurrentWorkplace.CurrentPieChartOptions.VisiblyOptions;

                Settings.Default.Save();
            }
            catch (Exception)
            {}
        }


        public ComTrade Data
        {
            set
            {
                WorkplaceControl newWorkplace = new WorkplaceControl();
                newWorkplace.Data = value;
                this.DocumentPanel.Content = newWorkplace;
                newWorkplace.MoveMarker += this.Workplace_MoveMarker;
                this.CurrentWorkplace = newWorkplace;
                this.CurrentWorkplace.Squeeze();
            }
        }
        
        private WorkplaceControl _currentWorkplace;

        private WorkplaceControl CurrentWorkplace
        {
            get { return this._currentWorkplace; }
            set
            {
                if (this._currentWorkplace != null)
                {
                    if (this._currentWorkplace.Data.IsEqual(value.Data))
                    {
                        this.VectorChart.Channels = this._currentWorkplace.Data.AnalogChannels;
                        this.FrequencyChart.Channels = this._currentWorkplace.Data.AnalogChannels;
                        this.PieChart.CurrentWorkplace = this._currentWorkplace;
                        this.RunOscCb.IsChecked = this._currentWorkplace.RunOscVisible;
                        this._currentWorkplace = value;
                        return;
                    }
                    
                }
                this._currentWorkplace = value;
                this._currentWorkplace.MarkerStatusBarControl = this.CommonStatusBar;
                if (this._currentWorkplace.Data.AnalogChannels.Length != 0)
                {
                    this.VectorChart.Channels = this._currentWorkplace.Data.AnalogChannels;
                    this.FrequencyChart.Channels = this._currentWorkplace.Data.AnalogChannels;
                    this.PieChart.CurrentWorkplace = this._currentWorkplace;
                }
                this.RunOscCb.IsChecked = this._currentWorkplace.RunOscVisible;
            }
        }

        string[] _hdr;
        public MainWindow()
        {
            this.InitializeComponent();
            
            new LayoutAnchorablePaneVisibleBinding(this.VectorCheckBox, this.VectorPanel);
            new LayoutAnchorablePaneVisibleBinding(this.PieCheckBox, this.PiePanel);
            new LayoutAnchorablePaneVisibleBinding(this.FreqCharCheckBox, this.FrequencyPanel);

            var bind = new CommandBinding(ApplicationCommands.Open);
            bind.Executed += this.OpenMenuItem_Click;
            this.CommandBindings.Add(bind);
            bind = new CommandBinding(ApplicationCommands.Close);
            bind.Executed += (sender, args) => this.Close();
            this.CommandBindings.Add(bind);
            bind = new CommandBinding(ApplicationCommands.SaveAs);
            bind.Executed += this.SaveAsMenuItem_Click;
            this.CommandBindings.Add(bind);

            this.VectorChart.MoveMarker += this.ChartOnMoveMarker;
            this.PieChart.MoveMarker += this.ChartOnMoveMarker;
            try
            {
                this._hdr = File.ReadAllLines(Path.ChangeExtension(App.BasePath, "hdr"));
                string ttl = this._hdr[0];
                int lenght = this._hdr.Length;
                if (!int.TryParse(this._hdr[lenght - 1], out this._encodding) || this._encodding != 1251)
                {
                    this._encodding = 0;
                }
                Title = ttl;
            }
            catch (Exception)
            {
                this._encodding = 0;
            }
            this.Data = ComTrade.Load(App.BasePath, this._encodding);
        }

        private void ChartOnMoveMarker(int obj)
        {
            if (this._currentWorkplace!= null)
            {
                 this._currentWorkplace.MainWindow_MoveMarker(obj);
            }
        }

        private void Workplace_MoveMarker(int obj)
        {
            if(this.CurrentWorkplace.Data.AnalogChannels.Length == 0) return;
            this.FrequencyChart.Set(obj);
            this.VectorChart.SetGraph(obj);
            this.PieChart.SetGraph(obj);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.ShowChannelOptions();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.ShowFactorForm();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.PrimaryFactor = true;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.PrimaryFactor = false;
        }

        private string _openInitialDirectory;
        private string _saveInitialDirectory;

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.InitialDirectory = this._openInitialDirectory;
            _openFileDialog.Filter = "Файл com-trade|*.cfg";
            bool? res = _openFileDialog.ShowDialog(this);

            if (res.GetValueOrDefault())
            {
                this._openInitialDirectory = Path.GetDirectoryName(_openFileDialog.FileName);
    
                try
                {
                    ComTrade file = ComTrade.Load(_openFileDialog.FileName, this._encodding);
                    this.Data = file;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка. Файл не может быть загружен" );
                }
            }
        }


        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = this._saveInitialDirectory;
            dialog.Filter = "Файл com-trade|*.cfg";
            if (this.CurrentWorkplace!= null)
            {
               dialog.FileName = this.CurrentWorkplace.Data.FileName; 
            }
            
            if (dialog.ShowDialog().GetValueOrDefault() && this.CurrentWorkplace != null)
            {
                this._saveInitialDirectory = Path.GetDirectoryName(dialog.FileName);
                File.AppendAllLines(Path.ChangeExtension(dialog.FileName,"hdr"), this._hdr);
                this.CurrentWorkplace.Data.Save(dialog.FileName, this._encodding);
            }
        }

        private void PART_MAXIMIZE_RESTORE_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void PART_MINIMIZE_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void increaseXButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.IncreaseX();
        }

        private void increaseYButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.IncreaseY();
        }

        private void decreaseXButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.DecreaseX();
        }

        private void decreaseYButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.DecreaseY();
        }

        private void normalizationButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace != null) this._currentWorkplace.OneZoom();
            
        }
        
        class LayoutAnchorablePaneVisibleBinding
        {
            private CheckBox _checkBox;
            private LayoutAnchorable _panel;

            public LayoutAnchorablePaneVisibleBinding(CheckBox checkBox, LayoutAnchorable panel)
            {
                this._checkBox = checkBox;
                this._panel = panel;
                checkBox.Checked += this.checkBox_Checked;
                checkBox.Unchecked += this.checkBox_Unchecked;
                panel.IsVisibleChanged += this.panel_IsVisibleChanged;
                panel.IsVisible = (bool) checkBox.IsChecked;
            }

            private void panel_IsVisibleChanged(object sender, EventArgs e)
             {
                 this._checkBox.IsChecked = this._panel.IsVisible;
             }
            
             void checkBox_Unchecked(object sender, RoutedEventArgs e)
             {
                 try
                 {
                     this._panel.IsVisible = false;
                 }
                 catch (Exception)
                 {
                 }
             }

             private void checkBox_Checked(object sender, RoutedEventArgs e)
             {
                 try
                 {
                     this._panel.IsVisible = true;
                 }
                 catch (Exception)
                 {
                 }
                
             }
        }
        
        private void RibbonButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace == null)
            {
                MessageBox.Show("Осциллограмма не загружена");
                return;
            }
            CutForm cutForm = new CutForm();
            cutForm.Marker1 = this.CurrentWorkplace.MarkersBar.Markers.Marker1;
            cutForm.Marker2 = this.CurrentWorkplace.MarkersBar.Markers.Marker2;
            cutForm.Min = 0;
            cutForm.Max = this._currentWorkplace.Data.Cfg.Size;
            if (cutForm.ShowDialog().Value)
            {
                ComTrade a = this.CurrentWorkplace.Data.Copy(cutForm.OscName, cutForm.Start, cutForm.End);
                this.Data = a;
            }
        }

        private void RibbonButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (this._currentWorkplace== null)
            {
                MessageBox.Show("Осциллограмма не загружена");
                return;
            }
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "Файл com-trade|*.cfg";
            bool? res = _openFileDialog.ShowDialog(this);
            if (res.GetValueOrDefault())
            {
                try
                {
                    var hdr = File.ReadAllLines(Path.ChangeExtension(App.BasePath, "hdr"));
                   int lenght = hdr.Length;
                    this._encodding = Convert.ToInt32(hdr[lenght - 1]);
                    ComTrade file = ComTrade.Load(_openFileDialog.FileName, this._encodding);
                    if (this._currentWorkplace.Data.IsEqual(file))
                    {
                        ComTrade newComtrade = this._currentWorkplace.Data.Add(file);
                        this.Data = newComtrade;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    this._encodding = 0;
                    ComTrade file = ComTrade.Load(_openFileDialog.FileName, this._encodding);
                    if (this._currentWorkplace.Data.IsEqual(file))
                    {
                        ComTrade newComtrade = this._currentWorkplace.Data.Add(file);
                        this.Data = newComtrade;
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("Ошибка. Файл не может быть загружен");
                }
            }
        }

        private void RunOscCb_Unchecked(object sender, RoutedEventArgs e)
        {
            this._currentWorkplace.RunOscVisible = false;
        }

        private void RunOscCb_Checked(object sender, RoutedEventArgs e)
        {
            this._currentWorkplace.RunOscVisible = true;
        }

        private void DiscretAllButton_Checked(object sender, RoutedEventArgs e)
        {
            CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State = CommonDiscretChartItems.CommonDiscretChart.States.ALL;
        }

        private void DiscretEnabledButton_Checked(object sender, RoutedEventArgs e)
        {
            CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State = CommonDiscretChartItems.CommonDiscretChart.States.ENABLED;
        }

        private void DiscretDisabledButton_Checked(object sender, RoutedEventArgs e)
        {
            CommonDiscretChartItems.CommonDiscretChart.VisibleOption.State = CommonDiscretChartItems.CommonDiscretChart.States.HIDE;
        }

        private void ActivValuesButton_Checked(object sender, RoutedEventArgs e)
        {
            AnalogChart.ValueType= AnalogChannel.GrafType.ACTIV;
        }

        private void RmsValuesButton_Checked(object sender, RoutedEventArgs e)
        {
             AnalogChart.ValueType= AnalogChannel.GrafType.RMS;
        }

        private void FirstHarmonicValuesButton_Checked(object sender, RoutedEventArgs e)
        {
             AnalogChart.ValueType= AnalogChannel.GrafType.FIRST_HARMONIC;
        }

        private void MinCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChart chart in this.CurrentWorkplace.AnalogCharts)
            {
                chart.VisibilityMin = this.MinCheckBox.IsChecked.GetValueOrDefault();
            }
            foreach (AnalogChart chart in this.CurrentWorkplace.AddedCharts)
            {
                chart.VisibilityMin = this.MinCheckBox.IsChecked.GetValueOrDefault();
            }
        }

        private void MaxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChart chart in this.CurrentWorkplace.AnalogCharts)
            {
                chart.VisibilityMax = this.MaxCheckBox.IsChecked.GetValueOrDefault();
            }
            foreach (AnalogChart chart in this.CurrentWorkplace.AddedCharts)
            {
                chart.VisibilityMax = this.MaxCheckBox.IsChecked.GetValueOrDefault();
            }
        }

        private void RmsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChart chart in this.CurrentWorkplace.AnalogCharts)
            {
                chart.VisibilityRms = this.RmsCheckBox.IsChecked.GetValueOrDefault();
            }
            foreach (AnalogChart chart in this.CurrentWorkplace.AddedCharts)
            {
                chart.VisibilityRms = this.RmsCheckBox.IsChecked.GetValueOrDefault();
            }
        }

        private void FirstHarmocCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChart chart in this.CurrentWorkplace.AnalogCharts)
            {
                chart.VisibilityHarminy = this.FirstFarmocCheckBox.IsChecked.GetValueOrDefault();
            }
            foreach (AnalogChart chart in this.CurrentWorkplace.AddedCharts)
            {
                chart.VisibilityHarminy = this.FirstFarmocCheckBox.IsChecked.GetValueOrDefault();
            }
        }

        private void FrequencyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (AnalogChart chart in this.CurrentWorkplace.AnalogCharts)
            {
                chart.VisibilityFrequency = this.FrequencyCheckBox.IsChecked.GetValueOrDefault();
            }
            foreach (AnalogChart chart in this.CurrentWorkplace.AddedCharts)
            {
                chart.VisibilityFrequency = this.FrequencyCheckBox.IsChecked.GetValueOrDefault();
            }
        }
    }
}
