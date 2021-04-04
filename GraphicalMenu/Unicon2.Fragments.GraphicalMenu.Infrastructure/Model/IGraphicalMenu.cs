using Unicon2.Infrastructure.FragmentInterfaces;

namespace Unicon2.Fragments.GraphicalMenu.Infrastructure.Model
{
    public interface IGraphicalMenu : IDeviceFragment
    {
        int DisplayWidth { get; set; }

        int DisplayHeight { get; set; }

        int CellWidth { get; set; }

        int CellHeight { get; set; }
    }
}