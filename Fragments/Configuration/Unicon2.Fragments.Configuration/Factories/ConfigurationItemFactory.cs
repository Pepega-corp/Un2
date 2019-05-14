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
            this._container = container;
        }

        #region Implementation of IConfigurationItemFactory

        public IConfigurationItem ResolveConfigurationItem()
        {
            IProperty prop = this._container.Resolve(typeof(IProperty)) as IProperty;
            prop.NumberOfPoints = 1;
            return prop;
        }

        public IConfigurationItem ResolveGroupConfigurationItem()
        {
            IItemsGroup group = this._container.Resolve(typeof(IItemsGroup)) as IItemsGroup;
            return group;
        }

        public IConfigurationItem ResolveDependentConfigurationItem()
        {
            IDependentProperty dependentProperty = this._container.Resolve(typeof(IDependentProperty)) as IDependentProperty;
            return dependentProperty;
        }

        public IConfigurationItem ResolveSubPropertyItem()
        {
            ISubProperty subProperty = this._container.Resolve(typeof(ISubProperty)) as ISubProperty;
            return subProperty;
        }

        public IConfigurationItem ResolveComplexPropertyItem()
        {
            IComplexProperty complexProperty = this._container.Resolve(typeof(IComplexProperty)) as IComplexProperty;
            complexProperty.NumberOfPoints = 1;
            return complexProperty;
        }

        public IConfigurationItem ResolveAppointableMatrix()
        {
            IConfigurationItem appointableMatrix = this._container.Resolve<IConfigurationItem>(ConfigurationKeys.APPOINTABLE_MATRIX);

            return appointableMatrix;
        }

        #endregion


    }
}
