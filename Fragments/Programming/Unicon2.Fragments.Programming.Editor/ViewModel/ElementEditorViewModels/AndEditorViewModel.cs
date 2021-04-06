
using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class AndEditorViewModel : ViewModelBase, IAndEditorViewModel
    {
        public string StrongName => ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public object Model { get; set; }
        public bool IsEditable => false;
        public string ElementName => "И";
        public string Symbol => "&";
        public string Description => "Логический элемент И";

        public AndEditorViewModel(IAndEditor model)
        {
            Model = model;
        }

        public object Clone()
        {
            return new AndEditorViewModel(new AndEditor());
        }
    }
}
