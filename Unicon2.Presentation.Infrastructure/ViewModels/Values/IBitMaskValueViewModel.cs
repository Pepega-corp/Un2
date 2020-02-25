using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IBitMaskValueViewModel : IFormattedValueViewModel<IBitMaskValue>
    {
        List<List<bool>> BitArray { get; set; }
    }
}