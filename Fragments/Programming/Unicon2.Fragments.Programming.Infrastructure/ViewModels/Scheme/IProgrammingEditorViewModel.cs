using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface IProgrammingEditorViewModel : IFragmentEditorViewModel
    {
        /// <summary>
        /// Все доступные элементы булевой логики
        /// </summary>
        ObservableCollection<ILogicElementEditorViewModel> BooleanElements { get; }
        /// <summary>
        /// Все доступные элементы 
        /// </summary>
        ObservableCollection<ILogicElementEditorViewModel> AnalogElements { get; }
    }
}