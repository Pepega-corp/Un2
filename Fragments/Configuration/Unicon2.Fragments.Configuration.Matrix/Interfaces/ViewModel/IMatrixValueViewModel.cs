using System;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.ViewModel
{
    public interface IMatrixValueViewModel : ICloneable
    {
        bool IsEditable { get; }
    }
}