using Unicon2.Presentation.Infrastructure.Enums;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Windows
{
    public class AnchorableWindowBase : ViewModelBase, IAnchorableWindow
    {
        public AnchorableWindowBase()
        {
            IsVisible = true;
        }

        private bool _isVisible;

        public string WindowNameKey { get; internal set; }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
        }

        public PlacementEnum AnchorableDefaultPlacementEnum { get; internal set; }
    }
}
