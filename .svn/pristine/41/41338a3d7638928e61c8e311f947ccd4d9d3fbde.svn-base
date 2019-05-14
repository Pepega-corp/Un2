using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.BaseItems;

namespace Unicon2.Infrastructure.Interfaces.Resourcres
{
    [DataContract]
    public abstract class DeviceResourceBase:Disposable,IResource
    {
        #region Implementation of ICloneable

        public abstract object Clone();

        #endregion

        #region Implementation of INameable
        [DataMember(Name = nameof(Name))]

        public string Name { get; set; }

        #endregion

        #region Implementation of IResource
        [DataMember(Name = nameof(IsAddedGlobal))]
        public bool IsAddedGlobal { get; set; }

        [DataMember(Name = nameof(ItemGuid))]
        public Guid ItemGuid { get; set; }
        

        #endregion

        #region Implementation of IStronglyNamed
        public abstract string StrongName { get; }
        #endregion
    }
}
