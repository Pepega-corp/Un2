using System;
using System.Collections.Generic;
using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.DependentProperty
{
    public interface IConditionViewModel:IViewModel,IDisposable
    {
        ICommand SelectPropertyFromResourceCommand { get; }
        string ReferencedResorcePropertyName { get; }

        List<string> ConditionsList { get; set; }
        string SelectedCondition { get; set; }
        ushort UshortValueToCompare { get; set; }
        List<string> ConditionResultList { get; set; }
        string SelectedConditionResult { get; set; }
        string UshortFormatterString { get;  }
        ICommand ShowFormatterParameters { get; }
    }
}