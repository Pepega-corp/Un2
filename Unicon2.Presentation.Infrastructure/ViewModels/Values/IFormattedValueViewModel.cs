using System;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IFormattedValueViewModel : IStronglyNamed, IRangeable, IMeasurable, IViewModel
    {
        string Header { get; set; }
        void InitFromValue(IFormattedValue value);

    }
}