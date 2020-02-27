using System.Runtime.Serialization;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Infrastructure.Values.Base
{
    [DataContract]
    public abstract class FormattedValueBase : IFormattedValue
    {
        public abstract string StrongName { get; }
        [DataMember]
        public string Header { get; set; }
        public abstract string AsString();
        public abstract T Accept<T>(IValueVisitor<T> visitor);
    }
}