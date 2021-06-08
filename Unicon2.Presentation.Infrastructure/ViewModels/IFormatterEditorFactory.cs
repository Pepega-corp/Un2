using System.Collections.Generic;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
  public interface IFormatterEditorFactory
    {
        void EditFormatterByUser(List<IUshortFormattableEditorViewModel> ushortFormattableViewModels,List<IConfigurationItemViewModel> rootConfigurationItemViewModels);

        void EditFormatterByUser(IUshortFormattableEditorViewModel ushortFormattableViewModel, List<IConfigurationItemViewModel> rootConfigurationItemViewModels);
    }
}
