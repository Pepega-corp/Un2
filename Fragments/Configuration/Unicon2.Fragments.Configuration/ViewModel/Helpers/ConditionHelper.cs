using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.ViewModel.Helpers
{
    public static class ConditionHelper
    {

        public static Result<bool> CheckCondition(ICompareCondition condition,ushort ushortToCompare)
        {
            switch (condition.ConditionsEnum)
            {
                case ConditionsEnum.Equal:
                    return Result<bool>.Create(ushortToCompare == condition.UshortValueToCompare, true);
                case ConditionsEnum.HaveFalseBitAt:
                    return Result<bool>.Create(!ushortToCompare.GetBoolArrayFromUshort()[condition.UshortValueToCompare], true);
                case ConditionsEnum.NotEqual:
                    return Result<bool>.Create(ushortToCompare != condition.UshortValueToCompare, true);
                case ConditionsEnum.More:
                    return Result<bool>.Create(ushortToCompare > condition.UshortValueToCompare, true);
                case ConditionsEnum.Less:
                    return Result<bool>.Create(ushortToCompare < condition.UshortValueToCompare, true);
                case ConditionsEnum.LessOrEqual:
                    return Result<bool>.Create(ushortToCompare <= condition.UshortValueToCompare, true);
                case ConditionsEnum.MoreOrEqual:
                    return Result<bool>.Create(ushortToCompare >= condition.UshortValueToCompare, true);
                case ConditionsEnum.HaveTrueBitAt:
                    return Result<bool>.Create(ushortToCompare.GetBoolArrayFromUshort()[condition.UshortValueToCompare], true);
                default:
                    throw new ArgumentOutOfRangeException();
            }
		}
    }
}
