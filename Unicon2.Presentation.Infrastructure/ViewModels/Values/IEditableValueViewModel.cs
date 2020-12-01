using System;
using System.ComponentModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IEditableValueViewModel : IFormattedValueViewModel, IUniqueId, INotifyDataErrorInfo,IDisposable
    {
        bool IsFormattedValueChanged { get; set; }
        bool IsEditEnabled { get; set; }
        void InitDispatcher(IDeviceEventsDispatcher deviceEventsDispatcher);
        T Accept<T>(IEditableValueViewModelVisitor<T> visitor);
        IFormattedValue FormattedValue { get; set; }
        
    }
}