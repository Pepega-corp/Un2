using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.Factories;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Factories
{
    public class ConfigurationItemFactory : IConfigurationItemFactory
    {
        private ITypesContainer _container;

        public ConfigurationItemFactory(ITypesContainer container)
        {
            _container = container;
        }

        public IConfigurationItem ResolveConfigurationItem()
        {
            IProperty prop = _container.Resolve(typeof(IProperty)) as IProperty;
            prop.NumberOfPoints = 1;
            return prop;
        }

        public IConfigurationItem ResolveGroupConfigurationItem()
        {
            IItemsGroup group = _container.Resolve(typeof(IItemsGroup)) as IItemsGroup;
            return group;
        }

        public IConfigurationItem ResolveDependentConfigurationItem()
        {
            IDependentProperty dependentProperty = _container.Resolve(typeof(IDependentProperty)) as IDependentProperty;
            return dependentProperty;
        }

        public IConfigurationItem ResolveSubPropertyItem()
        {
            ISubProperty subProperty = _container.Resolve(typeof(ISubProperty)) as ISubProperty;
            return subProperty;
        }

        public IConfigurationItem ResolveComplexPropertyItem()
        {
            IComplexProperty complexProperty = _container.Resolve(typeof(IComplexProperty)) as IComplexProperty;
            complexProperty.NumberOfPoints = 1;
            return complexProperty;
        }

        public IConfigurationItem ResolveAppointableMatrix()
        {
            IConfigurationItem appointableMatrix = _container.Resolve<IConfigurationItem>(ConfigurationKeys.APPOINTABLE_MATRIX);

            return appointableMatrix;
        }

        public IGroupWithReiterationInfo ResolveGroupWithReiterationInfo()
        {
            IGroupWithReiterationInfo groupWithReiterationInfo = _container.Resolve<IGroupWithReiterationInfo>();
            groupWithReiterationInfo.SubGroups=new List<IReiterationSubGroupInfo>();
            return groupWithReiterationInfo;
        }

        public IReiterationSubGroupInfo ResolveReiterationSubGroupInfo()
        {
            IReiterationSubGroupInfo reiterationSubGroupInfo = _container.Resolve<IReiterationSubGroupInfo>();
            return reiterationSubGroupInfo;
        }
    }
}
