using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.MockConnection.Module
{
    public class MockConnectionModule : IUnityModule
    {
        #region Implementation of IUnityModule

        public void Initialize(ITypesContainer container)
        {
            //регистрация ресурсов
            IXamlResourcesService xamlResourcesService = container.Resolve<IXamlResourcesService>();
            xamlResourcesService.AddResourceAsGlobal("Resources/MockConnectionResources.xaml", this.GetType().Assembly);
        }

        #endregion
    }
}
