using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oscilloscope.View.PieChartItem.Characteristics
{
[Serializable]
    public class PieChartChargeCharacteristicOption : ICharacteristic
    {
        static Pen DeepPinkPen = new Pen(Brushes.DeepPink, 1);
        public bool Enabled { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double F { get; set; }

    public void FromXml(XElement element)
    {
        Enabled = Convert.ToBoolean(element.Attribute("Enabled").Value, CultureInfo.InvariantCulture);
        R1 = Convert.ToDouble(element.Attribute("R1").Value, CultureInfo.InvariantCulture);
        R2 = Convert.ToDouble(element.Attribute("R2").Value, CultureInfo.InvariantCulture);
        F = Convert.ToDouble(element.Attribute("F").Value, CultureInfo.InvariantCulture);
    }
        public XElement ToXml()
        {
            return new XElement(this.GetType().ToString(),
                                new XAttribute("Enabled", Enabled.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("R1", R1.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("R2", R2.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("F", F.ToString(CultureInfo.InvariantCulture))
                );
        }


        static PieChartChargeCharacteristicOption()
        {
            DeepPinkPen.Freeze();
        }
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,"Нагрузка R1={0} R2={1} \u03C6={2}", R1, R2, F);
        }


        public void Draw(System.Windows.Media.DrawingContext context, Point mid, double factor, double radius)
        {
 
            var tg = Math.Tan((this.F)/180.0*Math.PI);
            var p1 = new Point(mid.X + radius, mid.Y - tg * radius );
            var p2 = new Point(R1 * factor + mid.X,mid.Y- tg * R1 * factor  );
            var p3 = new Point(R1 * factor + mid.X, mid.Y + tg * R1 * factor );
            var p4 = new Point(mid.X + radius, mid.Y + tg *  radius);
            context.DrawLine(DeepPinkPen,p1,p2);
            context.DrawLine(DeepPinkPen, p2, p3);
            context.DrawLine(DeepPinkPen, p3, p4);


            p1 = new Point(mid.X - radius, mid.Y - tg * radius);
            p2 = new Point(mid.X-R2 * factor  , mid.Y - tg * R2 * factor);
            p3 = new Point(mid.X-R2 * factor  , mid.Y + tg * R2 * factor);
            p4 = new Point(mid.X - radius, mid.Y + tg * radius);
            context.DrawLine(DeepPinkPen, p1, p2);
            context.DrawLine(DeepPinkPen, p2, p3);
            context.DrawLine(DeepPinkPen, p3, p4);
        }

    [XmlIgnore]
        public System.Windows.Point MaxPoint
        {
            get { return new Point(0,0);}
        }
    }
}