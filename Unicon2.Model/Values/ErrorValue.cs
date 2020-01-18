using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

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

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}