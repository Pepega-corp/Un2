using System;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels
{
    public interface ILogicElementEditorViewModel : IViewModel, ICloneable
    {
        string ElementName { get; }
        string Symbol { get; }
        string Description { get; }
        bool IsEditable { get; }
    }
}