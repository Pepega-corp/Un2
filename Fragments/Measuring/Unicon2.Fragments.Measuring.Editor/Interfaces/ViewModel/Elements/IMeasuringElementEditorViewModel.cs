using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements
{
    public interface IMeasuringElementEditorViewModel:IViewModel
    {
        string Header { get; set; }
        string NameForUiKey { get; }
    }
}