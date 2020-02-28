using System;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IMatrixValueViewModel : ICloneable
    {
        bool IsEditable { get; }
    }
}