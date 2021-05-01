using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels
{
    public class XorEditorViewModel : IXorEditorViewModel
    {
        public string StrongName => ProgrammingKeys.XOR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
        public object Model { get; set; }
        public string ElementName =>  "Искл. ИЛИ";
        public string Symbol => "^";
        public string Description => "Логический элемент исключающее ИЛИ";
        public bool IsEditable => false;
        
        public XorEditorViewModel(IXorEditor model)
        {
            Model = model;
        }
        
        public object Clone()
        {
            return new XorEditorViewModel(new XorEditor());
        }
    }
}