﻿namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IStringValueViewModel : IEditableValueViewModel
    {
        string StringValue { get; set; }
    }
}