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

        public FormattingService(ITypesContainer typesContainer)
        {
            _typesContainer = typesContainer;
            _iterationDefinitionsCache = new ConcurrentDictionary<string, ConcurrentBag<IterationDefinition>>();
        }


        public async Task<IFormattedValue> FormatValueAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, FormattingContext formattingContext)
        {
            try
            {
                return await TryFormatAsync(ushortsFormatter, ushorts, formattingContext);
            }
            catch (Exception e)
            {
                var error = _typesContainer.Resolve<IErrorValue>();
                error.ErrorMessage = e.Message;
                return error;
            }
        }

        private Task<IFormattedValue> TryFormatAsync(IUshortsFormatter ushortsFormatter, ushort[] ushorts, FormattingContext formattingContext)
        {
            if (ushortsFormatter == null)
            {
                return null;
            }
          
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache,formattingContext));
        }
        
        public async Task<Result<ushort[]>> FormatBackAsync(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue,FormattingContext formattingContext)
        {
            try
            {
                return Result<ushort[]>.Create(await ushortsFormatter.Accept(new FormatterFormatBackVisitor(formattedValue,formattingContext)),true);
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