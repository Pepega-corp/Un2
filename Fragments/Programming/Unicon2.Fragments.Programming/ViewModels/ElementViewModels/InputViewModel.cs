using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class InputViewModel : LogicElementViewModel
    {
        public InputViewModel() : base(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL)
        {
            ElementName = "Вход";
            Description = "Елемент входного дискретного сигнала";
            Symbol = "In";
        }
    }
}
