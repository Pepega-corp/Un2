using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Helpers
{
    public static class GroupInfoHelper
    {
        public static bool IfGroupInfoWithReiterationEnabled(IItemsGroup itemsGroup)
        {
            return (itemsGroup.GroupInfo is IGroupWithReiterationInfo groupWithReiterationInfo &&
                    groupWithReiterationInfo.IsReiterationEnabled);
        }
    }
}
