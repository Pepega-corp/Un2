using System;
using System.Globalization;
using System.Xml.Linq;

namespace Oscilloscope.View.PieChartItem
{
    [Serializable]
    public class ZnOptions
    {
        public double R0 { get; set; }
        public double R1 { get; set; }
        public double X0 { get; set; }
        public double X1 { get; set; }

        public double Kr { get { return /*(R0/R1 - 1)/3.0;*/ (R0*R1 + X0*X1)/(R1*R1 + X1*X1) - 1; } }
        public double Kx { get { return /*(X0 / X1 - 1) / 3.0;*/ (R1*X0 - R0*X1)/(R1*R1 + X1*X1); } }

        public void FromXml(XElement element)
        {
            R0 = Convert.ToDouble(element.Attribute("R0").Value, CultureInfo.InvariantCulture);
            R1 = Convert.ToDouble(element.Attribute("R1").Value, CultureInfo.InvariantCulture);
            X0 = Convert.ToDouble(element.Attribute("X0").Value, CultureInfo.InvariantCulture);
            X1 = Convert.ToDouble(element.Attribute("X1").Value, CultureInfo.InvariantCulture);
        }
        public XElement ToXml(string name)
        {
            return new XElement(name,
                                new XAttribute("R0", R0.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("R1", R1.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("X0", X0.ToString(CultureInfo.InvariantCulture)),
                                new XAttribute("X1", X1.ToString(CultureInfo.InvariantCulture))
                );
        }
    }
}