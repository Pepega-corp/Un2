using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IGroupWithReiterationInfo:IGroupInfo
    {
        int ReiterationStep { get; set; }

        List<IReiterationSubGroupInfo> SubGroups { get; set; }
    }

    public interface IGroupInfo
    {

    }

    public interface IReiterationSubGroupInfo
    {
        string Name { get; set; }
    }
}