using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Factories
{
    public class LogicElementFactory : ILogicElementFactory
    {
        private readonly ITypesContainer _container;

        public LogicElementFactory(ITypesContainer container)
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


        public List<ILogicElementViewModel> GetBooleanElementsViewModels()
        {
            var booleanElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();
            return GetAllElementsViewModels(booleanElements);
        }

        public List<ILogicElementViewModel> GetAnalogElementsViewModels()
        {
            var analogElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            return GetAllElementsViewModels(analogElements);
        }

        public List<ILogicElementViewModel> GetAllElementsViewModels(List<ILogicElement> elements)
        {
            return elements.Select(element =>
                StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL,
                    new ResolverParameter("model", element))).ToList();
        }

        public List<ILogicElementViewModel> GetAllElementsViewModels(List<ILibraryElement> libraryElements)
        {
            var allElements = this._container.ResolveAll<ILogicElement>().ToArray();
            var elements = new List<ILogicElement>();

            foreach (var libraryElement in libraryElements)
            {
                var element = allElements.FirstOrDefault(logicElement => logicElement.ElementType == libraryElement.ElementType);
                if (element != null)
                {
                    element.CopyLibraryValues(libraryElement);
                    elements.Add(element);
                }
            }

            return GetAllElementsViewModels(elements);
        }
    }
}
