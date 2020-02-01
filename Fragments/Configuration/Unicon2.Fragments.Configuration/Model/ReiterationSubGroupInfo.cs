using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Namespace = "ReiterationSubGroupInfoNS", Name = nameof(ReiterationSubGroupInfo), IsReference = true)]

    public class ReiterationSubGroupInfo: IReiterationSubGroupInfo
    {
        [DataMember]
        public string Name { get; set; }
    }
}