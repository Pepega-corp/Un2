using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ILogicElementEditorFactory
    {
        List<ILogicElementEditorViewModel> GetBooleanElementsEditorViewModels();
        List<ILogicElementEditorViewModel> GetAnalogElementsEditorViewModels();
        List<ILogicElementEditorViewModel> GetAllElementsEditorViewModels(List<ILibraryElement> elements);
    }
}