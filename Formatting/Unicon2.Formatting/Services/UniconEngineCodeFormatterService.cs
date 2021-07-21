using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Services.ExpressionEngine;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
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

        private ConcurrentDictionary<string, Func<BuiltExpressionFormatContext, Task<IFormattedValue>>> _formatCache =
            new ConcurrentDictionary<string, Func<BuiltExpressionFormatContext, Task<IFormattedValue>>>();

        private ConcurrentDictionary<string, Func<IFormattedValue, RuleExecutionContext, Task<ushort[]>>>
            _formatBackCache =
                new ConcurrentDictionary<string, Func<IFormattedValue, RuleExecutionContext, Task<ushort[]>>>();


        public UniconEngineCodeFormatterService(ILocalizerService localizerService)
        {

            _lexemManager = new LexemManager(localizerService);

            //     var str = "Add(2,Add(GetDeviceValue(0),2))";

        }


        public Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>> GetFormatUshortsFunc(
            CodeFormatterExpression codeExpression)
        {
            try
            {
                if (_formatCache.TryGetValue(codeExpression.CodeStringFormat, out var cachedFunc))
                {
                    Task<IFormattedValue> FunFromCacheWithContext(BuiltExpressionFormatContext context) =>
                        cachedFunc(context);

                    return Result<Func<BuiltExpressionFormatContext, Task<IFormattedValue>>>.Create(
                        FunFromCacheWithContext, true);
                }


                var nodes = Evaluator.Initialize(codeExpression.CodeStringFormat, _lexemManager);

                _formatCache.TryAdd(codeExpression.CodeStringFormat,
                    (context) => Evaluator.ExecuteFormat(context.DeviceValue,
                        new RuleExecutionContext(context.DeviceContext, context.IsLocal).SetVariable(
                            VariableNames.CURRENT_VALUE_OWNER, context.FormattedValueOwner), nodes));

                Func<BuiltExpressionFormatContext, Task<IFormattedValue>> fun = (context) =>
                    Evaluator.ExecuteFormat(context.DeviceValue,
                        new RuleExecutionContext(context.DeviceContext, context.IsLocal).SetVariable(
                            VariableNames.CURRENT_VALUE_OWNER, context.FormattedValueOwner), nodes);

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
            DeviceContext deviceContext, bool isLocal, IFormattedValueOwner formattedValueOwner)
        {
            try
            {
                if (_formatBackCache.TryGetValue(codeExpression.CodeStringFormatBack, out var cachedFunc))
                {
                    Task<ushort[]> FunFromCacheWithContext(IFormattedValue input) => cachedFunc(input,
                        new RuleExecutionContext(deviceContext, isLocal).SetVariable(VariableNames.CURRENT_VALUE_OWNER,
                            formattedValueOwner));

                    return Result<Func<IFormattedValue, Task<ushort[]>>>.Create(FunFromCacheWithContext, true);
                }

                var nodes = Evaluator.Initialize(codeExpression.CodeStringFormatBack, _lexemManager);

                Func<IFormattedValue, Task<ushort[]>> fun = (input) =>
                    Evaluator.ExecuteFormatBack(input,
                        new RuleExecutionContext(deviceContext, isLocal).SetVariable(VariableNames.CURRENT_VALUE_OWNER,
                            formattedValueOwner), nodes);

                _formatBackCache.TryAdd(codeExpression.CodeStringFormatBack,
                    (input, context) => Evaluator.ExecuteFormatBack(input, context, nodes));


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