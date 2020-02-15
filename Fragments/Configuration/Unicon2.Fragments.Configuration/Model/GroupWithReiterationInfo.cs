using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Helpers;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Namespace = "GroupWithReiterationInfoNS", Name = nameof(GroupWithReiterationInfo),
        IsReference = true)]
    public class GroupWithReiterationInfo : IGroupWithReiterationInfo
    {
        private IDataProvider _dataProvider;
        [DataMember] public int ReiterationStep { get; set; }

        [DataMember] public List<IReiterationSubGroupInfo> SubGroups { get; set; }

        [DataMember] public bool IsReiterationEnabled { get; set; }

        public object Clone()
        {
            var clone = new GroupWithReiterationInfo
            {
                IsReiterationEnabled = IsReiterationEnabled,
                ReiterationStep = ReiterationStep,
                SubGroups = SubGroups.Select((info => info.Clone() as IReiterationSubGroupInfo)).ToList()
            };
            return clone;
        }
    }
}