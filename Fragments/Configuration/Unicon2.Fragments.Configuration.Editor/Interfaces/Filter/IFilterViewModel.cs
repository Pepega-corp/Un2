using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Filter
{
    public interface IFilterViewModel:ICloneable<IFilterViewModel>
    {
        string Name { get; set; }
        ObservableCollection<IConditionViewModel> ConditionViewModels { get; }

    }
}