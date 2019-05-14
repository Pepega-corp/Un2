using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Editor.Interfaces
{
    public interface IProgrammingEditorViewModel : IFragmentEditorViewModel
    {
        /// <summary>
        /// Все доступные элементы булевой логики
        /// </summary>
        ObservableCollection<ILogicElementViewModel> BooleanElements { get; }
        /// <summary>
        /// Все доступные элементы 
        /// </summary>
        ObservableCollection<ILogicElementViewModel> AnalogElements { get; }
    }
}