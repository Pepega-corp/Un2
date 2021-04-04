using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.GraphicalMenu.Infrastructure.ViewModel
{
    public interface IGraphicalMenuFragmentEditorViewModel: IFragmentEditorViewModel
    {
        int DisplayWidth { get; set; }

        int DisplayHeight { get; set; }

        int CellWidth { get; set; }

        int CellHeight { get; set; }
    }
}