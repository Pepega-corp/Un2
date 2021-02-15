using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces
{
    public interface IBaseValuesViewModel
    {
        ObservableCollection<IBaseValueViewModel> BaseValuesViewModels { get; }
    }

    public interface IBaseValueViewModel
    {
        string Name { get; set; }
        Result<Dictionary<ushort,ushort>> BaseValuesMemory { get; set; }
        bool IsBaseValuesMemoryFilled { get; set; }
    }
}