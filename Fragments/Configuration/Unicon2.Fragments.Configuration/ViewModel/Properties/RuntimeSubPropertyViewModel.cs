using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.ViewModel.Properties
{
    public class RuntimeSubPropertyViewModel :  RuntimePropertyViewModel,IRuntimeSubPropertyViewModel
    {
        protected override string GetTypeName()
        {
            return ConfigurationKeys.SUB_PROPERTY;
        }
    }
}
