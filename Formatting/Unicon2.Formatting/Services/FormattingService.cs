using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Model;
using Unicon2.Formatting.Visitors;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Services
{

    public class FormattingService : IFormattingService
    {

        private readonly ITypesContainer _typesContainer;
        private ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>> _iterationDefinitionsCache;
        private ConcurrentDictionary<IUshortsFormatter, Func<BuiltExpressionFormatContext, Task<IFormattedValue>>> _codeFormatterCache;

        public FormattingService(ITypesContainer typesContainer)
        {
            _typesContainer = typesContainer;
            _iterationDefinitionsCache = new ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>>();
            _codeFormatterCache=new ConcurrentDictionary<IUshortsFormatter, Func<BuiltExpressionFormatContext, Task<IFormattedValue>>>();
        }


        public async Task<IFormattedValue> FormatValueAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, DeviceContext deviceContext, bool isLocal)
        {
            try
            {
                return await TryFormatAsync(ushortsFormatter, ushorts, deviceContext,isLocal);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return error;
            }
        }

        private async Task<IFormattedValue> TryFormatAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, DeviceContext deviceContext,bool isLocal)
        {
            if (ushortsFormatter == null)
            {
                return null;
            }
            if (ushortsFormatter is FormulaFormatter formulaFormatter)
            {
                return await (new FormatterFormatVisitor(ushorts, this._typesContainer,
                    this._iterationDefinitionsCache,isLocal,_codeFormatterCache).VisitFormulaFormatterAsync(ushortsFormatter, deviceContext));
            }

            if (ushortsFormatter is CodeFormatter codeFormatter)
            {
                return await (new FormatterFormatVisitor(ushorts, this._typesContainer,
                    this._iterationDefinitionsCache,isLocal,_codeFormatterCache).VisitCodeFormatterAsync(ushortsFormatter, deviceContext,isLocal));
            }
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache,isLocal,_codeFormatterCache));
        }

        public IFormattedValue FormatValue(IUshortsFormatter ushortsFormatter, ushort[] ushorts, bool isLocal)
        {
            try
            {
                return TryFormat(ushortsFormatter, ushorts,isLocal);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return error;
            }
        }

        private IFormattedValue TryFormat(IUshortsFormatter ushortsFormatter, ushort[] ushorts, bool isLocal)
        {
	        if (ushortsFormatter == null)
	        {
		        return null;
	        }
            if (ushortsFormatter is CodeFormatter codeFormatter)
            {
                INumericValue numValue = _typesContainer.Resolve<INumericValue>();
                numValue.NumValue = ushorts[0];
                return numValue;
            }
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache,isLocal,_codeFormatterCache));
        }

        public ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue, bool isLocal)
        {
            return ushortsFormatter.Accept(new FormatterFormatBackVisitor(formattedValue));
        }

        public async Task<Result<ushort[]>> FormatBackAsync(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue,DeviceContext deviceContext,bool isLocal=true)
        {
            try
            {
                if (ushortsFormatter is CodeFormatter codeFormatter)
                {
                    return await (new FormatterFormatBackVisitor(formattedValue).VisitCodeFormatterAsync(ushortsFormatter, deviceContext, isLocal));
                }
                return Result<ushort[]>.Create(ushortsFormatter.Accept(new FormatterFormatBackVisitor(formattedValue)),true);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return Result<ushort[]>.CreateWithException(e);
            }
         
        }
    }
}