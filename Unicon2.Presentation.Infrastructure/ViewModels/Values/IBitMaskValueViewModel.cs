using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IBitMaskValueViewModel : IFormattedValueViewModel
    {
        List<List<bool>> BitArray { get; set; }
    }
}