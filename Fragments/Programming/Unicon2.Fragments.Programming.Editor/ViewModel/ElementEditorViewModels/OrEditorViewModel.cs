using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class OrEditorViewModel : ViewModelBase, ILogicElementEditorViewModel
    {
        public string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public object Model { get; set; }
        public bool IsEditable => false;
        public string ElementName => "ИЛИ";
        public string Symbol => "|";
        public string Description => "Логический элемент ИЛИ";

        public OrEditorViewModel(ILibraryElement model)
        {
            Model = model;
        }

        public object Clone()
        {
            return new OrEditorViewModel(new OrEditor());
        }
    }
}
