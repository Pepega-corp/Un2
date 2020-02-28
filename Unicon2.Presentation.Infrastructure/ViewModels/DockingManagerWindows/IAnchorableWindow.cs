using Unicon2.Presentation.Infrastructure.Enums;

namespace Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows
{
    public interface IAnchorableWindow : IDockingWindow
    {
        bool IsVisible { get; set; }
        PlacementEnum AnchorableDefaultPlacementEnum { get; }
    }
}