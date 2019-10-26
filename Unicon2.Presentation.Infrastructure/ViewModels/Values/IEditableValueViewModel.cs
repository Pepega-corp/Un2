using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IEditableValueViewModel : IFormattedValueViewModel,IViewModel
    {
        bool IsFormattedValueChanged { get; }
        void SetBaseValueToCompare(ushort[] ushortsToCompare);
        void SetUshortFormatter(IUshortsFormatter ushortsFormatter);
        Action<ushort[]> ValueChangedAction { get; set; }
        bool IsEditEnabled { get; set; }
        
    }
}