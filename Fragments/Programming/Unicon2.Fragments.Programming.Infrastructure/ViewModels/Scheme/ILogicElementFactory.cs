using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ILogicElementFactory
    {
        List<ILogicElementEditorViewModel> GetBooleanElementsEditorViewModels();
        List<ILogicElementViewModel> GetBooleanElementsViewModels();
        List<ILogicElementEditorViewModel> GetAnalogElementsEditorViewModels();
        List<ILogicElementViewModel> GetAnalogElementsViewModels();
        List<ILogicElementEditorViewModel> GetAllElementsEditorViewModels(ILibraryElement[] elements);
        List<ILogicElementViewModel> GetAllElementsViewModels(ILogicElement[] elements);
    }
}