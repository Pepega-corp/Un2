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
        [DataMember]
        public List<IConfigurationItem> ConfigurationItems { get; set; }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            ConfigurationItems?.ForEach(item=>item.SetDataProvider(dataProvider));
        }

        public async Task<bool> Write()
        {
            var isWritten = false;
            foreach (var item in ConfigurationItems)
            {
                if (await item.Write()) isWritten = true;
            }
            return isWritten;
        }

        public async Task Load()
        {
            foreach (var configurationItem in ConfigurationItems)
            {
               await configurationItem.Load();
            }
        }
    }
}