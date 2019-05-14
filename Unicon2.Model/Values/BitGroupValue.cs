using System.Runtime.Serialization;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class BitGroupValue:IBitGroupValue
    {
        #region Implementation of IBitGroupValue
        [DataMember]
        public IFormattedValue FormattedValue { get; set; }
        [DataMember]
        public IUshortsFormatter UshortsFormatter { get; set; }

        #endregion
    }
}
