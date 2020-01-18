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
        public PieChannelInfo(Point[] values ,VisibilityItem visibleItem)
        {
            _visibleItem = visibleItem;
            _channelName = visibleItem.Channel;
       
            Values = values;

            _channelPen = new Pen(visibleItem.LineColor, 1);
            _channelPen.Freeze();
        }

        private Pen _channelPen ;
        public Pen ChannelPen
        {
            get
            {
                return _channelPen;
            }
        }
        public string ChannelName
        {
            get { return _channelName; }
            set
            {
                _channelName = value;
                RaisePropertyChanged("ChannelName");
            }
        }

        public Brush LineColor
        {
            get { return _visibleItem.LineColor; }
            set
            {
                _visibleItem.LineColor = value;
                RaisePropertyChanged("LineColor");
            }
        }

        public string X
        {
            get { return AnalogChannel.Normalize(this.Values[_index].Y, "Îì"); }
            set
            {
                RaisePropertyChanged("X");
            }
        }

        public string R
        {
            get { return AnalogChannel.Normalize(this.Values[_index].X, "Îì"); }
            set
            {
                RaisePropertyChanged("R");
            }
        }

        private int _index = 0;
        public void SetIndex(int index)
        {
            _index = index;
            this.X = "";
            this.R = "";
        }
        public Point[] Values { get; private set; }
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private VisibilityItem _visibleItem;




        public bool IsVisibly
        {
            get
            {
                return _visibleItem.Visible;
            }
            set
            {
                _visibleItem.Visible = value;
            }
        }

        public MenuItem ChannelMenuItem
        {
          get
          {
              if (_menuItem == null)
                {
                    _menuItem = new MenuItem();
                    _menuItem.IsCheckable = true;
                    _menuItem.Header = _channelName;
                    var binding = new Binding();
                    binding.Source = _visibleItem;
                    binding.Path = new PropertyPath("Visible");
                    _menuItem.SetBinding(MenuItem.IsCheckedProperty, binding);

                }
                return _menuItem;
          } 
        }
    }
}