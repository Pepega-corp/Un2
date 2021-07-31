using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;

namespace Unicon2.Fragments.Programming.Editor
{
    public class DragEditorElement
    {
        public ILogicElementEditorViewModel Item { get; set; }
        public DragEditorElement(ILogicElementEditorViewModel item)
        {
            this.Item = item;
        }
    }
}
