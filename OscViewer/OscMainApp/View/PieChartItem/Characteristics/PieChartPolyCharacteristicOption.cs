using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oscilloscope.View.PieChartItem.Characteristics
{
 [Serializable]
    public class PieChartPolyCharacteristicOption : ICharacteristic,INotifyPropertyChanged
    {
        static Pen BrownPen = new Pen(Brushes.Brown,1);
     private double _r;
     public bool Enabled { get; set; }
        public double R
        {
            get
            {
                return _r;
            }
            set
            {
               _r = value;
                if (PropertyChanged!= null)
                {
                    this.PropertyChanged.Invoke(this,new PropertyChangedEventArgs("R"));
                }
            }
        }

     public double X { get; set; }
        public double F { get; set; }

        static PieChartPolyCharacteristicOption()
        {
            BrownPen.Freeze();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,"Полигональная R={0} X={1} \u03C6={2}", R, X, F);
        }
        public void FromXml(XElement element)
        {

            Enabled = Convert.ToBoolean(element.Attribute("Enabled").Value, CultureInfo.InvariantCulture);
            R = Convert.ToDouble(element.Attribute("R").Value, CultureInfo.InvariantCulture);
            X = Convert.ToDouble(element.Attribute("X").Value, CultureInfo.InvariantCulture);
            F = Convert.ToDouble(element.Attribute("F").Value, CultureInfo.InvariantCulture);
        }
        public XElement ToXml()
        {

            return new XElement(this.GetType().ToString(),
                                new XAttribute("Enabled", Enabled.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("R", R.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("X", X.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("F", F.ToString(CultureInfo.InvariantCulture))
                );
        }
        public void Draw(DrawingContext context, Point mid, double factor, double radius)
        {
           
                  

            var p1 = new Point(mid.X - this.R * factor,
                                            mid.Y - this.X * factor);
            var p2 = new Point(mid.X + MaxPoint.X * factor, mid.Y - this.X * factor);
            var p3 = new Point(mid.X + this.R * factor, mid.Y);
            var p4 = new Point(mid.X + this.R * factor,
                               mid.Y + this.X * factor);
            var p5 = new Point(mid.X - MaxPoint.X * factor, mid.Y + this.X * factor);
            var p6 = new Point(mid.X - this.R * factor, mid.Y);
            context.DrawLine(BrownPen, p1, p2);
            context.DrawLine(BrownPen, p2, p3);
            context.DrawLine(BrownPen, p3, p4);
            context.DrawLine(BrownPen, p4, p5);
            context.DrawLine(BrownPen, p5, p6);
            context.DrawLine(BrownPen, p6, p1);
        }

     [XmlIgnore]
        public Point MaxPoint
        {
            get { return new Point(this.R + this.X*Math.Tan((90 - this.F)/180.0*Math.PI), this.X); }
        }

     public event PropertyChangedEventHandler PropertyChanged;
    }
}