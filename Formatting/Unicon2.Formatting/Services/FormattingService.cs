using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Unicon2.Formatting.Model;
using Unicon2.Formatting.Visitors;
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

        public FormattingService(ITypesContainer typesContainer)
        {
            _typesContainer = typesContainer;
            _iterationDefinitionsCache = new ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>>();
        }


        public async Task<IFormattedValue> FormatValueAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, DeviceContext deviceContext)
        {
            try
            {
                return await TryFormatAsync(ushortsFormatter, ushorts, deviceContext);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return error;
            }
        }
        private async Task<IFormattedValue> TryFormatAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, DeviceContext deviceContext)
        {
            if (ushortsFormatter == null)
            {
                return null;
            }
            if (ushortsFormatter is FormulaFormatter formulaFormatter)
            {
                return await (new FormatterFormatVisitor(ushorts, this._typesContainer,
                    this._iterationDefinitionsCache).VisitFormulaFormatterAsync(ushortsFormatter, deviceContext));
            }
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache));
        }

        public IFormattedValue FormatValue(IUshortsFormatter ushortsFormatter, ushort[] ushorts)
        {
            try
            {
                return TryFormat(ushortsFormatter, ushorts);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return error;
            }
        }

        private IFormattedValue TryFormat(IUshortsFormatter ushortsFormatter, ushort[] ushorts)
        {
	        if (ushortsFormatter == null)
	        {
		        return null;
	        }
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache));
        }

        public ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue)
        {
            return ushortsFormatter.Accept(new FormatterFormatBackVisitor(formattedValue));
        }
    }
}