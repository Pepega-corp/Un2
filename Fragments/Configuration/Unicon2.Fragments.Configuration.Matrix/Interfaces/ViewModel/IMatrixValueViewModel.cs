using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.ViewModel
{
    public interface IMatrixValueViewModel:IViewModel
    {
        bool IsEditable { get; }
    }
}