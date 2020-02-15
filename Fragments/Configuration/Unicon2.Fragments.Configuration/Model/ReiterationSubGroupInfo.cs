using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Namespace = "ReiterationSubGroupInfoNS", Name = nameof(ReiterationSubGroupInfo), IsReference = true)]

    public class ReiterationSubGroupInfo: IReiterationSubGroupInfo
    {

        [DataMember]
        public string Name { get; set; }
        
        public object Clone()
        {
            return new ReiterationSubGroupInfo()
            {
                Name = Name,
            };
        }
    }
}