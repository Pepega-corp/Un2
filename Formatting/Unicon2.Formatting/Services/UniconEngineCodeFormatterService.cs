using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Services.ExpressionEngine;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Services
{

    public class UniconEngineCodeFormatterService : ICodeFormatterService
    {
        private LexemManager _lexemManager;

        public UniconEngineCodeFormatterService()
        {

            _lexemManager=new LexemManager();

            //     var str = "Add(2,Add(GetDeviceValue(0),2))";

        }


        public Result<Func<ushort[], Task<IFormattedValue>>> GetFormatUshortsFunc(
            CodeFormatterExpression codeExpression, DeviceContext deviceContext)
        {
            if (codeExpression.BuiltExpressionFormat.IsSuccess)
            {
                return codeExpression.BuiltExpressionFormat;
            }



            var nodes = Evaluator.Initialize(codeExpression.CodeStringFormat, _lexemManager);

            return Result<Func<ushort[], Task<IFormattedValue>>>.Create(
                (deviceValue) => Evaluator.ExecuteFormat(deviceValue, new RuleExecutionContext(deviceContext), nodes), true);

        }

        public Result<Func<IFormattedValue, Task<ushort[]>>> GetFormatBackUshortsFunc(CodeFormatterExpression codeExpression,
            DeviceContext deviceContext)
        {
            if (codeExpression.BuiltExpressionFormatBack.IsSuccess)
            {
                return codeExpression.BuiltExpressionFormatBack;
            }

            var nodes = Evaluator.Initialize(codeExpression.CodeStringFormat, _lexemManager);

            return Result<Func<IFormattedValue, Task<ushort[]>>>.Create(
                (input) => Evaluator.ExecuteFormatBack(input, new RuleExecutionContext(deviceContext), nodes), true);
        }
    }
}