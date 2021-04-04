using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.GraphicalMenu.ViewModel
{
    public class GraphicalMenuSlotViewModel:ViewModelBase
    {
        private bool _isHighlighted;

        public GraphicalMenuSlotViewModel(int slotLeftOffset, int slotTopOffset)
        {
            SlotLeftOffset = slotLeftOffset;
            SlotTopOffset = slotTopOffset;
        }

        public int SlotLeftOffset { get; }
        public int SlotTopOffset { get; }
        public GraphicalElementViewModel RelatedGraphicalElementViewModel { get; set; }

        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                if (_isHighlighted == value) return;
                _isHighlighted = value;
                RaisePropertyChanged();
            }
        }
    }
}
