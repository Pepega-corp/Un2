using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IGroupWithReiterationInfo : IGroupInfo, ILoadable, IWriteable
    {
        int ReiterationStep { get; set; }

        List<IReiterationSubGroupInfo> SubGroups { get; set; }
        bool IsReiterationEnabled { get; set; }
        void SetGroupItems(List<IConfigurationItem> items);
    }

    public interface IGroupInfo
    {

    }


    public interface IReiterationSubGroupInfo: ILoadable,IWriteable
    {
        string Name { get; set; }
        List<IConfigurationItem> ConfigurationItems { get; set; }
    }
}