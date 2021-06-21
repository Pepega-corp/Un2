using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Services.ExpressionEngine;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Services
{

    public class UniconEngineCodeFormatterService : ICodeFormatterService
    {
        private LexemManager _lexemManager;

        public UniconEngineCodeFormatterService(ILocalizerService localizerService)
        {

            _lexemManager=new LexemManager(localizerService);

            //     var str = "Add(2,Add(GetDeviceValue(0),2))";

        }


        public Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>> GetFormatUshortsFunc(
            CodeFormatterExpression codeExpression)
        {
            try
            {
                var nodes = Evaluator.Initialize(codeExpression.CodeStringFormat, _lexemManager);

                Func<BuiltExpressionFormatContext, Task<IFormattedValue>> fun = (context) =>
                    Evaluator.ExecuteFormat(context.DeviceValue, new RuleExecutionContext(context.DeviceContext, context.IsLocal), nodes);

            //    codeExpression.BuiltExpressionFormat = Result<Func<ushort[], Task<IFormattedValue>>>.Create(fun,true);

                return Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>>.Create(fun,
                    true);
            }
            catch (Exception e)
            {
                return Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>>.CreateWithException(e);
            }
        }

        public Result<Func<IFormattedValue, Task<ushort[]>>> GetFormatBackUshortsFunc(
            CodeFormatterExpression codeExpression,
            DeviceContext deviceContext, bool isLocal)
        {
            try
            {


                var nodes = Evaluator.Initialize(codeExpression.CodeStringFormatBack, _lexemManager);

                Func<IFormattedValue, Task<ushort[]>> fun = (input) =>
                    Evaluator.ExecuteFormatBack(input, new RuleExecutionContext(deviceContext, isLocal), nodes);

            //    codeExpression.BuiltExpressionFormatBack =
            //        Result<Func<IFormattedValue, Task<ushort[]>>>.Create(fun, true);


                return Result<Func<IFormattedValue, Task<ushort[]>>>.Create(fun, true);
            }
            catch (Exception e)
            {
                return Result<Func<IFormattedValue, Task<ushort[]>>>.CreateWithException(e);

            }
        }

        public List<(string name, string desc)> GetFunctionsInfo()
        {
            return _lexemManager.KnownLexemaVisitors.Select(pair => (pair.Key, pair.Value.description)).ToList();
        }
    }
}