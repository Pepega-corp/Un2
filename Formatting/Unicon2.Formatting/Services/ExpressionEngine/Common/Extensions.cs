using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Formatting.Services.ExpressionEngine.Common
{
    public static class Extensions
    {
        public static List<string> GetParameterFromString(this string str)
        {

            var resultList = new List<string>();

            string buffer = String.Empty;
            int parenthesisCount = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ',' && parenthesisCount==0)
                {
                    if (!string.IsNullOrWhiteSpace(buffer))
                    {
                        resultList.Add(buffer);
                    }
                    buffer = String.Empty;
                    continue;
                }

                if (str[i] == '(')
                {
                    parenthesisCount++;
                }
                if (str[i] == ')')
                {
                    parenthesisCount--;
                    if (parenthesisCount == 0)
                    {
                        buffer += str[i].ToString();
                        if (!string.IsNullOrWhiteSpace(buffer))
                        {
                            resultList.Add(buffer);
                        }
                        buffer = String.Empty;
                        continue;
                    }
                }
           /*     if (parenthesisCount==0&&str[i] == '+')
                {
                    if (!string.IsNullOrWhiteSpace(buffer))
                    {
                        resultList.Add(buffer);
                    }
                    buffer = String.Empty;

                    buffer += str[i].ToString();
                    if (!string.IsNullOrWhiteSpace(buffer))
                    {
                        resultList.Add(buffer);
                    }
                    buffer = String.Empty;
                    continue;
                }*/



                buffer += str[i].ToString();

            }

            if (!string.IsNullOrWhiteSpace(buffer))
            {
                resultList.Add(buffer);
            }
/*
            if (resultList.Contains("+"))
            {
                var index = resultList.IndexOf("+");
                resultList[index] = $"Add({resultList[index - 1]},{resultList[index + 1]})";
                resultList.RemoveAt(index+1);
                resultList.RemoveAt(index - 1);

            }

            if (resultList.Contains("-"))
            {
                var index = resultList.IndexOf("+");
                resultList[index] = $"Add({resultList[index - 1]},{resultList[index + 1]})";
                resultList.RemoveAt(index + 1);
                resultList.RemoveAt(index - 1);

            }

            if (resultList.Contains("/"))
            {
                var index = resultList.IndexOf("+");
                resultList[index] = $"Add({resultList[index - 1]},{resultList[index + 1]})";
                resultList.RemoveAt(index + 1);
                resultList.RemoveAt(index - 1);

            }

            if (resultList.Contains("/"))
            {
                var index = resultList.IndexOf("+");
                resultList[index] = $"Add({resultList[index - 1]},{resultList[index + 1]})";
                resultList.RemoveAt(index + 1);
                resultList.RemoveAt(index - 1);

            }
*/
            return resultList;
        }

        internal static OperatorType GetOperatorType(this string input)
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
        internal static FormatterType GetFormatterType(this string input)
        {
            switch (input)
            {
                case "number":
                case "Number":
                    return FormatterType.Number;
                case "Bool":
                case "bool":
                case "boolean":
                    return FormatterType.Bool;
                case "String":
                case "string":
                    return FormatterType.String;
            }
            return FormatterType.String;
        }
    }
}