using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel
{
    public interface IItemGroupViewModel : IConfigurationItemViewModel
    {
        bool IsTableViewAllowed { get; set; }
        bool IsMain { get; set; }
    }
}