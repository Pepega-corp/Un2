using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;

namespace Unicon2.Fragments.Programming.Other
{
    public class DragElement
    {
        public ILogicElementViewModel Item { get; set; }
        public DragElement(ILogicElementViewModel item)
        {
            this.Item = item;
        }
    }
}
