using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model;
using Unicon2.Formatting.Visitors;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Services.Formatting;
using Unicon2.Infrastructure.Values;
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
            return ushortsFormatter.Accept(new FormatterFormatVisitor(ushorts, _typesContainer,
                _iterationDefinitionsCache));
        }

        public ushort[] FormatBack(IUshortsFormatter ushortsFormatter, IFormattedValue formattedValue)
        {
            return ushortsFormatter.Accept(new FormatterFormatBackVisitor(formattedValue));
        }
    }
}