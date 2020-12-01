using System;
using System.ComponentModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IRangeViewModel : INotifyDataErrorInfo, ICloneable
    {
        string RangeFrom { get; set; }
        string RangeTo { get; set; }
    }
}