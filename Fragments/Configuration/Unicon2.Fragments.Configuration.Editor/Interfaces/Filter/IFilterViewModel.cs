using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Filter
{
    public interface IFilterViewModel
    {
        string Name { get; set; }
        ObservableCollection<IConditionViewModel> ConditionViewModels { get; }

    }
}