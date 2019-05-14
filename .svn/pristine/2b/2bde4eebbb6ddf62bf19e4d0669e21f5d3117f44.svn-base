using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace Oscilloscope.View.PieChartItem.Characteristics
{
   public interface ICharacteristic
    {
       bool Enabled { get; set; }
       void Draw(DrawingContext context, Point mid, double factor, double radius);
       Point MaxPoint { get; }
       XElement ToXml();
     void  FromXml(XElement element);
    }
}
