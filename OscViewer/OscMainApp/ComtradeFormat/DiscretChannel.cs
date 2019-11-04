using System.Linq;

namespace Oscilloscope.ComtradeFormat
{
   public class DiscretChannel
   {
       private DiscretChannelConfiguration _configuration;
       private int[] _data;
       private int _run;
       public bool[] Values { get; private set; }
       public string Name
       {
           get { return this._configuration.Name; }
       }
       public int Length
       {
           get { return Values == null ? 0 : Values.Length; }
       }

       public int[] Data
       {
           get { return _data; }
       }

       public DiscretChannel(DiscretChannelConfiguration configuration, int[] data, int run)
      {
          this._run = run;
          _configuration = configuration;
          _data = data;

          this.Values = data.Select(o => o!=0).ToArray();
           this.IsEmpty = data.All(o => o == 0);
      }


       public bool IsEmpty { get; private set; }
       public override string ToString()
       {
           return _configuration.ToString();
       }

       public int Run
       {
           get { return _run; }
       }
   }
}
