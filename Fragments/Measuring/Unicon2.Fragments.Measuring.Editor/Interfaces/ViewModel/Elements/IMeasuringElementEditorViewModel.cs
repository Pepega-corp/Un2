using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IMeasuringElementEditorViewModel : IUniqueIdWithSet
    {
        string Header { get; set; }
        string NameForUiKey { get; }
    }
}