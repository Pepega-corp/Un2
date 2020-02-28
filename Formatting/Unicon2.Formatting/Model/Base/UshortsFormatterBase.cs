using System;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model.Base
{
    [DataContract(IsReference = true, Namespace = "UshortsFormatterBaseNS")]
    public abstract class UshortsFormatterBase : Disposable, IUshortsFormatter
    {
        public abstract object Clone();

        [DataMember]
        public string Name { get; set; }

        public abstract T Accept<T>(IFormatterVisitor<T> visitor);
    }

}