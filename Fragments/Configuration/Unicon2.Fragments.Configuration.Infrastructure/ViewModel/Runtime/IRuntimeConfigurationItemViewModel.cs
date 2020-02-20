using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IRuntimeConfigurationItemViewModel : IConfigurationItemViewModel
    {
        T Accept<T>(IConfigurationItemVisitor<T> visitor);
    }
}
