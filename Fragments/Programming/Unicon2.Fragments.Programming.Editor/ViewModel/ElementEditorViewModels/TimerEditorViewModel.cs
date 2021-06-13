using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class TimerEditorViewModel : ViewModelBase, ILogicElementEditorViewModel
    {
        public string StrongName => ProgrammingKeys.TIMER +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model { get; set; }
        public bool IsEditable => false;
        public string ElementName => "Таймер";
        public string Symbol => "T";
        public string Description => "Логический элемент Таймер";

        public TimerEditorViewModel(ILibraryElement model)
        {
            Model = model;
        }

        public object Clone()
        {
            return new TimerEditorViewModel(new TimerEditor());
        }
    }
}
