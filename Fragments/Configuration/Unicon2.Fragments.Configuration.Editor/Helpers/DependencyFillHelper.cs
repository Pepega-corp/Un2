using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Presentation.Infrastructure.ViewModels.Dependencies;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class DependencyFillHelper
    {
        public static IDependencyViewModel CreateDependencyViewModel(IDependency dependency)
        {
            StaticContainer.Container.Resolve()
        }
        
        public static IDependency CreateDependencyModel(IDependencyViewModel dependency)
        {
            
        }
    }
}