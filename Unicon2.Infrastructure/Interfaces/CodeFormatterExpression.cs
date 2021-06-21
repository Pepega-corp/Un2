using System;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Infrastructure.Interfaces
{
    public class CodeFormatterExpression
    {
        public CodeFormatterExpression(string codeStringFormat, string codeStringFormatBack)
        {
            CodeStringFormat = codeStringFormat;
            CodeStringFormatBack = codeStringFormatBack;
        }

        public string CodeStringFormat { get; }
        public string CodeStringFormatBack { get; }
    }

}