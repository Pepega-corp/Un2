using System.Collections.Generic;

namespace Unicon2.Infrastructure.Values.Matrix.OptionTemplates
{
    public interface IOptionPossibleValue
    {
        string PossibleValueName { get; set; }
        List<IPossibleValueCondition> PossibleValueConditions { get; set; }
    }
}