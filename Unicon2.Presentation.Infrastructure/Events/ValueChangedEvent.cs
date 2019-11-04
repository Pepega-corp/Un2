using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Events
{
   public class ValueChangedEvent
    {
        public ILocalAndDeviceValueContainingViewModel LocalAndDeviceValueContainingViewModel { get; }
        public IFormattedValueViewModel NewValue { get; }
        public bool IsDeviceValue { get; }

        public ValueChangedEvent(ILocalAndDeviceValueContainingViewModel localAndDeviceValueContainingViewModel,IFormattedValueViewModel newValue,bool isDeviceValue)
        {
            LocalAndDeviceValueContainingViewModel = localAndDeviceValueContainingViewModel;
            NewValue = newValue;
            IsDeviceValue = isDeviceValue;
        }
    }
}
