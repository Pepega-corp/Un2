using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class ErrorValue:FormattedValueBase,IErrorValue
    {
        #region Overrides of FormattedValueBase

        public override string StrongName => nameof(ErrorValue);
        public override string AsString()
        {
            return this.ErrorMessage;
        }

        #endregion

        #region Implementation of IErrorValue
        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}