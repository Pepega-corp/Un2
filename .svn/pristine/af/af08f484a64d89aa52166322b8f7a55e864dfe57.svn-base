using Unicon2.Formatting.Infrastructure.Factories;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Factories
{
    public class FormatterFactory : IFormatterFactory
    {
        private readonly ITypesContainer _container;

        public FormatterFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IUshortsFormatter GetUshortsFormatterByKey(string formatterKey)
        {
            return this._container.Resolve<IUshortsFormatter>(formatterKey);
        }
    }
}
