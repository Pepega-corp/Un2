using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates
{
    public interface IOptionPossibleValue
    {
        string PossibleValueName { get; set; }
        List<IPossibleValueCondition> PossibleValueConditions { get; set; }
    }
}