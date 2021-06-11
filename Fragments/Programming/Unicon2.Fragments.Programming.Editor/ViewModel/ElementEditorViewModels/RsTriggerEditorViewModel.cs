using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class RsTriggerEditorViewModel : ViewModelBase, ILogicElementEditorViewModel
    {
        public string StrongName => ProgrammingKeys.RS_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public object Model { get; set; }
        public bool IsEditable => false;
        public string ElementName => "RS-триггер";
        public string Symbol => "RST";
        public string Description => "Логический элемент RS-триггер";

        public RsTriggerEditorViewModel(ILibraryElement model)
        {
            Model = model;
        }

        public object Clone()
        {
            return new RsTriggerEditorViewModel(new RsTrigger());
        }
    }
}
