using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;

namespace Unicon2.Fragments.Programming.Editor.Interfaces
{
    public interface ILogicElementFactory
    {
        List<ILogicElementViewModel> GetBooleanElementsViewModels();
        List<ILogicElementViewModel> GetAnalogElementsViewModels();
        List<ILogicElementViewModel> GetAllElementsViewModels(List<ILogicElement> elements);
    }
}