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
            BuiltExpressionFormat=Result<Func<ushort[], Task<IFormattedValue>>>.Create(false);
            BuiltExpressionFormatBack= Result<Func<IFormattedValue, Task<ushort[]>>>.Create(false);
        }

        public string CodeStringFormat { get; }
        public string CodeStringFormatBack { get; }

        public Result<Func<ushort[],Task<IFormattedValue>>> BuiltExpressionFormat { get;set; }
        public Result<Func<IFormattedValue, Task<ushort[]>>> BuiltExpressionFormatBack { get; set; }

    }
}