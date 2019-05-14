using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IFormattedValueViewModel:IStronglyNamed,IRangeable
    {
        string Header { get; set; }
        void InitFromValue(IFormattedValue value);
        Action<object,object> FormattedValueChanged { get; set; }
    }
}