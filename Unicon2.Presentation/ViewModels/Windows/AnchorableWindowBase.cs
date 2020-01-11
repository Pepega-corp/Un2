using Unicon2.Presentation.Infrastructure.Enums;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.ViewModels.Windows
{
    public class AnchorableWindowBase : ViewModelBase, IAnchorableWindow
    {
        public AnchorableWindowBase()
        {
            this.IsVisible = true;
        }

        private bool _isVisible;

        public string WindowNameKey { get; internal set; }

        public bool IsVisible
        {
            get { return this._isVisible; }
            set
            {
                this._isVisible = value;
                this.RaisePropertyChanged();
            }
        }

        public PlacementEnum AnchorableDefaultPlacementEnum { get; internal set; }
    }
}
