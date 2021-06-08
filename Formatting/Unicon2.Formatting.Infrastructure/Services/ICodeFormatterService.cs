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
       Result<Func<ushort[], Task<IFormattedValue>>> GetFormatUshortsFunc(CodeFormatterExpression codeExpression, DeviceContext deviceContext);
       Result<Func<IFormattedValue, Task<ushort[]>>> GetFormatBackUshortsFunc(CodeFormatterExpression codeExpression, DeviceContext deviceContext);


    }
}
