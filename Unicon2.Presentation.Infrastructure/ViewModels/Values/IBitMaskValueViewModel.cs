using System.Collections.Generic;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IBitMaskValueViewModel : IFormattedValueViewModel
    {
        List<List<bool>> BitArray { get; set; }
    }
}