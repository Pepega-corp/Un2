using System;
using System.ComponentModel;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IRangeViewModel : INotifyDataErrorInfo, ICloneable
    {
        string RangeFrom { get; set; }
        string RangeTo { get; set; }
    }
}