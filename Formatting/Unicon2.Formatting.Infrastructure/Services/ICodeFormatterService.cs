using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Infrastructure.Services
{
   public interface ICodeFormatterService
   {
       Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>> GetFormatUshortsFunc(CodeFormatterExpression codeExpression);
       Result<Func<IFormattedValue, Task<ushort[]>>> GetFormatBackUshortsFunc(CodeFormatterExpression codeExpression, DeviceContext deviceContext, bool isLocal);

       List<(string name, string desc)> GetFunctionsInfo();
   }

   public class BuiltExpressionFormatContext
   {
       public BuiltExpressionFormatContext(DeviceContext deviceContext, ushort[] deviceValue, bool isLocal)
       {
           DeviceContext = deviceContext;
           DeviceValue = deviceValue;
           IsLocal = isLocal;
       }

       public bool IsLocal { get; }
       public ushort[] DeviceValue { get; }
       public DeviceContext DeviceContext { get; }
   }
}
