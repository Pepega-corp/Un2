using System;
using System.Collections.ObjectModel;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.TreeGrid
{
    public interface IConfigurationItemViewModel
    {
        string Header { get; set; }

        int Level { get; set; }
        Action<bool?> Checked { get; set; }
        string TypeName { get; }
        bool IsChecked { get; set; }
        string Description { get; set; }
        bool IsCheckable { get; set; }

        ObservableCollection<IConfigurationItemViewModel> ChildStructItemViewModels { get; set; }
        IConfigurationItemViewModel Parent { get; set; }
    }
}