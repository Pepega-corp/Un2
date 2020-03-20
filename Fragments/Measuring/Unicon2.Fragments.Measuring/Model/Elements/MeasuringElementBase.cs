using System;
using System.Runtime.Serialization;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "MeasuringElementNS")]
    public abstract class MeasuringElementBase : IMeasuringElement
    {
        public abstract string StrongName { get; }

        [DataMember]
        public string Name { get; set; }

    }
}
