using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class ErrorValue:FormattedValueBase,IErrorValue
    {
        public override string StrongName => nameof(ErrorValue);
        public override string AsString()
        {
            return this.ErrorMessage;
        }
        public override T Accept<T>(IValueVisitor<T> visitor)
        {
	        return visitor.VisitErrorValue(this);
        }

		[DataMember]
        public string ErrorMessage { get; set; }
    }
}