using System.Runtime.Serialization;

namespace Unicon2.Infrastructure.Values.Base
{
    [DataContract]
    public abstract class FormattedValueBase : IFormattedValue
    {
        #region Implementation of IStronglyNamed


        public abstract string StrongName { get; }

        #endregion

        #region Implementation of IFormattedValue

        [DataMember]

        public string Header { get; set; }

        [DataMember]

        public ushort[] UshortsValue { get; set; }

        public abstract string AsString();

        #endregion


    }
}