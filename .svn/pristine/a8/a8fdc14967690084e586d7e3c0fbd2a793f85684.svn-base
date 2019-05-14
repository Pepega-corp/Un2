using System;
using System.Runtime.Serialization;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "MeasuringElementNS")]
    public abstract class MeasuringElementBase : IMeasuringElement
    {
        #region Implementation of IStronglyNamed

        public abstract string StrongName { get; }

        #endregion

        #region Implementation of INameable
        [DataMember]
        public string Name { get; set; }

        #endregion

        #region Implementation of IMeasuringElement

        public Action ElementChangedAction { get; set; }

        #endregion
    }
}
