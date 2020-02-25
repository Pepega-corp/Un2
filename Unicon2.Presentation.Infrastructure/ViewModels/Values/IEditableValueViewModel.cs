using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IEditableValueViewModel<TFormattableValue> : IFormattedValueViewModel<TFormattableValue>, IUniqueId
    {
        bool IsFormattedValueChanged { get; }
        TFormattableValue GetValue();
        bool IsEditEnabled { get; set; }
        void InitSubscription(IMemorySubscription memorySubscription);

    }
}