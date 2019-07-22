using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.HelperClasses
{
    public class DragElement
    {
        public ILogicElementViewModel Item { get; set; }
        public DragElement(ILogicElementViewModel item)
        {
            this.Item = item;
        }
    }

    public class DragEditorElement
    {
        public ILogicElementEditorViewModel Item { get; set; }
        public DragEditorElement(ILogicElementEditorViewModel item)
        {
            this.Item = item;
        }
    }
}
