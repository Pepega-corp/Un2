using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IMeasuringElementEditorViewModel
    {
        string Header { get; set; }
        string NameForUiKey { get; }
    }
}