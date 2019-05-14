using System.Globalization;
using System.Linq;

namespace Oscilloscope.ComtradeFormat
{
    public class DiscretChannelConfiguration
    {
        private int _number;
        private string _name;
        private int _default;

        public DiscretChannelConfiguration Clone()
        {
            return (DiscretChannelConfiguration)this.MemberwiseClone();
        }
        public string Name
        {
            get { return _name; }
        }
           public DiscretChannelConfiguration(string name, int defaultValue)
           {
               _name = name;
               _default = defaultValue;
           }

        public DiscretChannelConfiguration(string line)
        {
            string[] parameters = line.Split(new[] { ',' });
            if (parameters.Length == 3)
            {
                this._number = int.Parse(parameters[0]);
                this._name = parameters[1];
                this._default = int.Parse(parameters[2]);
            }
            else
            {
                this._number = int.Parse(parameters[0]);
                this._name = parameters[1];
                for (int i = 2; i < parameters.Length - 1; i++)
                {
                    this._name += "," + parameters[i];
                }
                this._default = int.Parse(parameters.Last());
            }

        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,"{0},{1}", this._name, this._default);
        }

        internal bool IsEqual(DiscretChannelConfiguration discretChannelConfiguration)
        {
            return this._name == discretChannelConfiguration._name &
                   this._default == discretChannelConfiguration._default;
        }
    }
}