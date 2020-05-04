using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty
{
    public interface IConditionViewModel : IStronglyNamed, IDisposable
    {
        ICommand SelectPropertyFromResourceCommand { get; }
        string ReferencedResourcePropertyName { get; set; }

        List<string> ConditionsList { get; set; }
        string SelectedCondition { get; set; }
        ushort UshortValueToCompare { get; set; }
        List<string> ConditionResultList { get; set; }
        string SelectedConditionResult { get; set; }
    }
}