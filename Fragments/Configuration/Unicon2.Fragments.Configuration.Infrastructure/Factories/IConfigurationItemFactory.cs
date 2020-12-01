using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.Factories
{
    public interface IConfigurationItemFactory
    {
        IConfigurationItem ResolveConfigurationItem();
        IConfigurationItem ResolveGroupConfigurationItem();

        IConfigurationItem ResolveSubPropertyItem();
        IConfigurationItem ResolveComplexPropertyItem();
        IConfigurationItem ResolveAppointableMatrix();
        IGroupWithReiterationInfo ResolveGroupWithReiterationInfo();
        IReiterationSubGroupInfo ResolveReiterationSubGroupInfo();

    }
}