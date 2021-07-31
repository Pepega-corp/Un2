using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;

namespace Unicon2.Fragments.Programming.Other
{
    public class DragElement
    {
        public LogicElementViewModel Item { get; set; }
        public DragElement(LogicElementViewModel item)
        {
            this.Item = item;
        }
    }
}
