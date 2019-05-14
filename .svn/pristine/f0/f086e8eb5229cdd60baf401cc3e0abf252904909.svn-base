using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oscilloscope.View.PieChartItem.Characteristics
{
  [Serializable]
    public class PieChartDirectionCharacteristicOption : ICharacteristic
    {
        static Pen DarkVioletPen = new Pen(Brushes.DarkViolet, 1);
        static Pen GoldPen = new Pen(Brushes.Gold, 1);
        public bool Enabled { get; set; }
        public double F1 { get; set; }
        public double F2 { get; set; }

        static PieChartDirectionCharacteristicOption()
        {
            DarkVioletPen.Freeze();
            GoldPen.Freeze();
        }
        public void FromXml(XElement element)
        {
            Enabled = Convert.ToBoolean(element.Attribute("Enabled").Value, CultureInfo.InvariantCulture);
            F1 = Convert.ToDouble(element.Attribute("F1").Value, CultureInfo.InvariantCulture);
            F2 = Convert.ToDouble(element.Attribute("F2").Value, CultureInfo.InvariantCulture);
        }
        public XElement ToXml()
        {
            return new XElement(this.GetType().ToString(),
                                new XAttribute("Enabled", Enabled.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("F1", F1.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("F2", F2.ToString(CultureInfo.InvariantCulture))
                );
        }
        public override string ToString()
        {        
            return string.Format(CultureInfo.InvariantCulture,"Направление \u03C61={0} \u03C62={1}", F1, F2);
        }


        public void Draw(System.Windows.Media.DrawingContext context, Point mid, double factor, double radius)
        {
            var tg1 = Math.Tan((this.F1)/180.0*Math.PI);

            var p1 = new Point(mid.X + radius, tg1 * radius + mid.Y);
            var p2 = new Point(mid.X - radius, mid.Y - tg1 * radius);


            var tg2 = Math.Tan((this.F2) / 180.0 * Math.PI);
            var p3 = new Point(tg2 * radius + mid.X, mid.Y + radius);
            var p4 = new Point(mid.X - tg2 * radius, mid.Y - radius);

            var midPoint = new Point(mid.X, mid.Y);
         context.DrawLine(DarkVioletPen, p1,midPoint );
         context.DrawLine(GoldPen, midPoint, p2);

         context.DrawLine(GoldPen, p3,midPoint);
         context.DrawLine(DarkVioletPen, midPoint, p4);
  
        }

      [XmlIgnore]
        public System.Windows.Point MaxPoint
        {
            get { return new Point(0, 0); }
        }
    }
}