using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Views;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
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

        #region Implementation of IFormatterEditorFactory

        public void EditFormatterByUser(IUshortFormattable model)
        {
            IApplicationGlobalCommands applicationGlobalCommands = this._container.Resolve<IApplicationGlobalCommands>();
            applicationGlobalCommands?.ShowWindowModal(() => new FormatterView(), new FormatterSelectionViewModel(this._container, model));
        }

        #endregion
    }
}
