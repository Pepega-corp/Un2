using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Subscription;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IEditableValueViewModel : IFormattedValueViewModel, IUniqueId
    {
        bool IsFormattedValueChanged { get; }
        bool IsEditEnabled { get; set; }
        void InitDispatcher(IDeviceEventsDispatcher deviceEventsDispatcher);
        T Accept<T>(IEditableValueViewModelVisitor<T> visitor);
        IFormattedValue FormattedValue { get; set; }
        void RefreshBaseValueToCompare();
        
    }
}