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

        public void SetGroupItems(List<IConfigurationItem> items)
        {
            if(SubGroups==null)return;
            SetDataProvider(_dataProvider);
            if (SubGroups.Any((info => info.ConfigurationItems == null)))
            {
                SubGroups?.ForEach(subGroup =>
                {
                    subGroup.ConfigurationItems = items.Select(item => item.Clone() as IConfigurationItem).ToList();
                });
            }
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            var counter = 0;
            foreach (var subGroup in SubGroups)
            {
                subGroup.SetDataProvider(new DataProviderAddressWrapper(dataProvider,
                    (ushort) (counter * ReiterationStep)));
                counter++;
            }
        }

        public async Task<bool> Write()
        {
            var isWritten = false;
            foreach (var subGroup in SubGroups)
            {
                if (await subGroup.Write()) isWritten = true;
            }
            return isWritten;
        }

        public async Task Load()
        {
            foreach (var subGroup in SubGroups)
            {
                await subGroup.Load();
            }
        }
    }
}