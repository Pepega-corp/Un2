using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels
{
    public interface ILogicElementViewModel : ISelectable, IViewModel
    {
        string ElementName { get; }
        string Caption { get; set; }
        string Description { get; }
        string Symbol { get; }
        bool ValidationError { get; set; }
        bool DebugMode { get; set; }
        object Clone();
    }
}