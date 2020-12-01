using Unicon.Backend.Common.Modularity;

namespace Unicon.Backend.WebApi.Module
{
    public class RegistriesInitializer
    {
        private readonly ITypesProvider _typesProvider;

        public RegistriesInitializer(ITypesProvider typesProvider)
        {
            _typesProvider = typesProvider;
        }


        public void Init(params IRegistry[] registries)
        {
            foreach (var registry in registries)
            {
                registry.Init(_typesProvider);
            }
        }
    }
}