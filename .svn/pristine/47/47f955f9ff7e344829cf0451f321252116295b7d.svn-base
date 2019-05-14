using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Oscilloscope.View.PieChartItem
{
    public class PieChannelInfo : INotifyPropertyChanged
    {
        private string _channelName = string.Empty;
        private MenuItem _menuItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public PieChannelInfo()
        {

        }
        public PieChannelInfo(Point[] values, VisibilityItem visibleItem)
        {
            this._visibleItem = visibleItem;
            this._channelName = visibleItem.Channel;

            this.Values = values;

            this.ChannelPen = new Pen(visibleItem.LineColor, 1);
            this.ChannelPen.Freeze();
        }

        public Pen ChannelPen { get; }

        public string ChannelName
        {
            get { return this._channelName; }
            set
            {
                this._channelName = value;
                this.RaisePropertyChanged("ChannelName");
            }
        }

        public Brush LineColor
        {
            get { return this._visibleItem.LineColor; }
            set
            {
                this._visibleItem.LineColor = value;
                this.RaisePropertyChanged("LineColor");
            }
        }

        public string X
        {
            get { return AnalogChannel.Normalize(this.Values[this._index].Y, "Îì"); }
            set
            {
                this.RaisePropertyChanged("X");
            }
        }

        public string R
        {
            get { return AnalogChannel.Normalize(this.Values[this._index].X, "Îì"); }
            set
            {
                this.RaisePropertyChanged("R");
            }
        }

        private int _index = 0;
        public void SetIndex(int index)
        {
            this._index = index;
            this.X = "";
            this.R = "";
        }
        public Point[] Values { get; private set; }

        private void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private VisibilityItem _visibleItem;


        public bool IsVisibly
        {
            get { return this._visibleItem.Visible; }
            set { this._visibleItem.Visible = value; }
        }

        public MenuItem ChannelMenuItem
        {
            get
            {
                if (this._menuItem == null)
                {
                    this._menuItem = new MenuItem();
                    this._menuItem.IsCheckable = true;
                    this._menuItem.Header = this._channelName;
                    Binding binding = new Binding();
                    binding.Source = this._visibleItem;
                    binding.Path = new PropertyPath("Visible");
                    this._menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);

                }
                return this._menuItem;
            }
        }
    }
}