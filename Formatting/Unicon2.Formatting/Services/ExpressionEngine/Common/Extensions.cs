using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Unicon2.Formatting.Services.ExpressionEngine.Common
{
    public static class Extensions
    {
        public static List<string> GetParameterFromString(this string str)
        {
            var matches = Regex.Split(str, @",\s*(?![^()]*\))");


            return matches.ToList();
        }

        public static OperatorType GetOperatorType(this string input)
        {
            switch (input)
            {
                case "=":
                    return OperatorType.Equal;
                case "==":
                    return OperatorType.Equal;
                case ">=":
                    return OperatorType.MoreOrEqual;
                case "<=":
                    return OperatorType.LessOrEqual;
                case "<":
                    return OperatorType.Less;
                case ">":
                    return OperatorType.More;
            }

            return OperatorType.Unknown;
        }
    }
}