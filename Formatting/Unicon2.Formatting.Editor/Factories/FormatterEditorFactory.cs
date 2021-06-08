using System.Collections.Generic;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.Factories
{
    public class FormatterEditorFactory : IFormatterEditorFactory
    {
        private readonly ITypesContainer _container;

        public FormatterEditorFactory(ITypesContainer container)
        {
            _container = container;
        }
        public void EditFormatterByUser(List<IUshortFormattableEditorViewModel> ushortFormattableViewModel, List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            IApplicationGlobalCommands applicationGlobalCommands =
                _container.Resolve<IApplicationGlobalCommands>();
            applicationGlobalCommands?.ShowWindowModal(() => new FormatterView(),
                new FormatterSelectionViewModel(_container, ushortFormattableViewModel,rootConfigurationItemViewModels));
        }

        public void EditFormatterByUser(IUshortFormattableEditorViewModel ushortFormattableViewModel, List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            IApplicationGlobalCommands applicationGlobalCommands =
                _container.Resolve<IApplicationGlobalCommands>();
            applicationGlobalCommands?.ShowWindowModal(() => new FormatterView(),
                new FormatterSelectionViewModel(_container, new List<IUshortFormattableEditorViewModel>(){ushortFormattableViewModel},rootConfigurationItemViewModels));
        }
    }
}