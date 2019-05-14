using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.Factories
{
    public interface IConfigurationItemFactory
    {
        IConfigurationItem ResolveConfigurationItem();
        IConfigurationItem ResolveGroupConfigurationItem();
        IConfigurationItem ResolveDependentConfigurationItem();

        IConfigurationItem ResolveSubPropertyItem();
        IConfigurationItem ResolveComplexPropertyItem();
        IConfigurationItem ResolveAppointableMatrix();

    }
}