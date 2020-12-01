using System;
using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IGroupWithReiterationInfo : IGroupInfo
    {
        int ReiterationStep { get; set; }

        List<IReiterationSubGroupInfo> SubGroups { get; set; }
        bool IsReiterationEnabled { get; set; }
    }

    public interface IGroupInfo : ICloneable
    {

    }


    public interface IReiterationSubGroupInfo : ICloneable
    {
        string Name { get; set; }
    }
}