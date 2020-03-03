using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.Factories
{
    public class FormatterEditorFactory : IFormatterEditorFactory
    {
        private readonly ITypesContainer _container;

        public FormatterEditorFactory(ITypesContainer container)
        {
            this._container = container;
        }


        public void EditFormatterByUser(IUshortFormattableEditorViewModel ushortFormattableViewModel)
        {
            IApplicationGlobalCommands applicationGlobalCommands =
                this._container.Resolve<IApplicationGlobalCommands>();
            applicationGlobalCommands?.ShowWindowModal(() => new FormatterView(),
                new FormatterSelectionViewModel(this._container, ushortFormattableViewModel));
        }
    }
}