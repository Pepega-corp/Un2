using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IFormattedValueViewModel<in TFormattedValue> : IRangeable, IMeasurable
    {
        string Header { get; set; }
        void InitFromValue(TFormattedValue value);

    }
}