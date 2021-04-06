using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class InversionEditorViewModel : ViewModelBase, IInversionEditorViewModel
    {
        public string StrongName => ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public object Model { get; set; }
        public bool IsEditable => false;
        public string ElementName => "НЕ";
        public string Symbol => "~";
        public string Description => "Логический элемент НЕ";

        public InversionEditorViewModel(IInversionEditor model)
        {
            Model = model;
        }
        
        public object Clone()
        {
            return new InversionEditorViewModel(new InversionEditor());
        }
    }
}
