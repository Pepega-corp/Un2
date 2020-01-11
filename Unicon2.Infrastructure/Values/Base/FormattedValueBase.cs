using System.Runtime.Serialization;

namespace Unicon2.Infrastructure.Values.Base
{
    [DataContract]
    public abstract class FormattedValueBase : IFormattedValue
    {
        public abstract string StrongName { get; }

        [DataMember]

        public string Header { get; set; }

        [DataMember]

        public ushort[] UshortsValue { get; set; }

        public abstract string AsString();
    }
}