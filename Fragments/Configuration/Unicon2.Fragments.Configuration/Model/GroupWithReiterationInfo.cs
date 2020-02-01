using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Properties;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Namespace = "GroupWithReiterationInfoNS", Name = nameof(GroupWithReiterationInfo), IsReference = true)]
    public class GroupWithReiterationInfo: IGroupWithReiterationInfo
    {
        [DataMember]
        public int ReiterationStep { get; set; }

        [DataMember]
        public List<IReiterationSubGroupInfo> SubGroups { get; set; }
    }
}