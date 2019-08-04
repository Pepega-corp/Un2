using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ILogicElementFactory
    {
        List<ILogicElementEditorViewModel> GetBooleanElementsViewModels();
        List<ILogicElementEditorViewModel> GetAnalogElementsViewModels();
        List<ILogicElementEditorViewModel> GetAllElementsViewModels(List<ILogicElement> elements);
    }
}