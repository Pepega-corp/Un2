using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Factories
{
    public class LogicElementsFactory
    {
        private readonly ITypesContainer _container;

        public LogicElementsFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public List<LogicElementViewModel> GetBooleanElementsViewModels()
        {
            var booleanElements = this._container.ResolveAll<LogicElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();
            return GetAllElementsViewModels(booleanElements);
        }

        public List<LogicElementViewModel> GetAnalogElementsViewModels()
        {
            var analogElements = this._container.ResolveAll<LogicElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            return GetAllElementsViewModels(analogElements);
        }

        public List<LogicElementViewModel> GetAllElementsViewModels(List<LogicElement> elements)
        {
            var ret = elements.Select(element =>
                StaticContainer.Container.Resolve<LogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL,
                    new ResolverParameter("model", element))).ToList();
            return ret;
        }

        public List<LogicElementViewModel> GetAllElementsViewModels(List<ILibraryElement> libraryElements)
        {
            var allElements = this._container.ResolveAll<LogicElement>().ToArray();
            var elements = new List<LogicElement>();

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
