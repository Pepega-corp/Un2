using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Factories
{
    public class LogicElementEditorFactory : ILogicElementEditorFactory
    {
        private readonly ITypesContainer _container;

        public LogicElementEditorFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public List<ILogicElementEditorViewModel> GetBooleanElementsEditorViewModels()
        {
            var booleanElements = this._container.ResolveAll<ILibraryElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();

            return GetAllElementsEditorViewModels(booleanElements);
        }

        public List<ILogicElementEditorViewModel> GetAnalogElementsEditorViewModels()
        {
            var analogElements = this._container.ResolveAll<ILibraryElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();

            return GetAllElementsEditorViewModels(analogElements);
        }

        public List<ILogicElementEditorViewModel> GetAllElementsEditorViewModels(List<ILibraryElement> elements)
        {
            return elements.Select(element =>
                StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL,
                    new ResolverParameter("model", element))).ToList();
        }
    }
}
