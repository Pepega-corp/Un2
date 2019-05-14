using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oscilloscope.View.PieChartItem
{
    [Serializable]
    public   class VisibilityItem : INotifyPropertyChanged
    {
        private string _channel;
        private bool _visible;
        private bool _r;
        private bool _x;
         [NonSerialized]
     
        private Brush _lineColor;

         public VisibilityItem(){}

        public VisibilityItem(string channel, Color color)
        {
            Channel = channel;
            Visible = true;
            R = false;
            X = false;
            _lineColor = new SolidColorBrush(color);
        }
         public XElement ToXml()
         {
             return new XElement(_channel,
                                 new XAttribute("Visible", _visible),
                                 new XAttribute("R", _r),
                                 new XAttribute("X", _x)

                 );
         }

         public void FromXml(XElement element)
         {
             element = element.Element(_channel);
             if (element == null)
             {
                 _visible = false;
                 _r = false;
                 _x = false;
                 return;
             }
             _visible = Convert.ToBoolean(element.Attribute("Visible").Value);
             _r = Convert.ToBoolean(element.Attribute("R").Value);
             _x = Convert.ToBoolean(element.Attribute("X").Value);   
         }
          [XmlIgnore]
        public Brush LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                RaisePropertyChanged("LineColor");
            }
        }

        
        public string Color
        {
        get { return ((SolidColorBrush) _lineColor).Color.ToString(); }
            set { LineColor = new SolidColorBrush((Color) ColorConverter.ConvertFromString(value)); }
        }

        public string Channel
        {
            get { return _channel; }
            set
            {
                RaisePropertyChanged("Channel");
                _channel = value;
            }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                RaisePropertyChanged("Visible"); 
                _visible = value;
            }
        }

        public bool R
        {
            get { return _r; }
            set
            {
                RaisePropertyChanged("R");
                _r = value;
            }
        }

        public bool X
        {
            get { return _x; }
            set
            {
                RaisePropertyChanged("X");
                _x = value;
            }
        }

        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
       [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
    }
}