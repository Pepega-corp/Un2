using System.Collections.Generic;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
  public interface IFormatterEditorFactory
    {
        void EditFormatterByUser(List<IUshortFormattableEditorViewModel> ushortFormattableViewModels);

        void EditFormatterByUser(IUshortFormattableEditorViewModel ushortFormattableViewModel);
    }
}
