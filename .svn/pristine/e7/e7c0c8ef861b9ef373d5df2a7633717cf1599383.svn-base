using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oscilloscope.View.PieChartItem.Characteristics
{
[Serializable]
    public class PieChartRoundCharacteristicOption : ICharacteristic
    {
        static Pen ChartreusePen = new Pen(Brushes.Chartreuse, 1);
        public bool Enabled { get; set; }
        public double R { get; set; }
        public double X { get; set; }
        public double Radius { get; set; }

        static PieChartRoundCharacteristicOption()
        {
            ChartreusePen.Freeze();
        }

        public override string ToString()
        {
           
            return string.Format(CultureInfo.InvariantCulture,"Круговая R={0} X={1} r={2}", R, X, Radius);
        }
        public void FromXml(XElement element)
        {
            Enabled = Convert.ToBoolean(element.Attribute("Enabled").Value, CultureInfo.InvariantCulture);
            R = Convert.ToDouble(element.Attribute("R").Value, CultureInfo.InvariantCulture);
            X = Convert.ToDouble(element.Attribute("X").Value, CultureInfo.InvariantCulture);
            Radius = Convert.ToDouble(element.Attribute("Radius").Value, CultureInfo.InvariantCulture);
        }
        public XElement ToXml()
        {
            return new XElement(this.GetType().ToString(),
                                new XAttribute("Enabled", Enabled.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("R", R.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("X", X.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("Radius", Radius.ToString(CultureInfo.InvariantCulture))
                );
        }
        public void Draw(DrawingContext context, Point mid, double factor, double radius)
        {
            context.DrawEllipse(null,ChartreusePen,new Point(mid.X +R*factor,mid.Y- X*factor),Radius*factor,Radius*factor );
        }

[XmlIgnore]
        public Point MaxPoint
        {
            get { return new Point(Math.Abs(R) + Radius, Math.Abs(X) + Radius); }
        }
    }
}