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
            var booleanElementViewModels = new List<ILogicElementEditorViewModel>();
            foreach (var element in booleanElements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                element.InitializeDefault();
                viewmodel.Model = element;
                booleanElementViewModels.Add(viewmodel);
            }

            return booleanElementViewModels;
        }

        public List<ILogicElementEditorViewModel> GetAnalogElementsEditorViewModels()
        {
            var analogElements = this._container.ResolveAll<ILibraryElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            var analogElementViewModels = new List<ILogicElementEditorViewModel>();
            foreach (var element in analogElements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName +ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                element.InitializeDefault();
                viewmodel.Model = element;
                analogElementViewModels.Add(viewmodel);
            }

            return analogElementViewModels;
        }

        public List<ILogicElementEditorViewModel> GetAllElementsEditorViewModels(List<ILibraryElement> elements)
        {
            var elementsViewModels = new List<ILogicElementEditorViewModel>();
            foreach (var element in elements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                elementsViewModels.Add(viewmodel);
            }

            return elementsViewModels;
        }


        public List<ILogicElementViewModel> GetBooleanElementsViewModels()
        {
            var booleanElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();
            var booleanElementViewModels = new List<ILogicElementViewModel>();
            foreach (var element in booleanElements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                booleanElementViewModels.Add(viewmodel);
            }

            return booleanElementViewModels;
        }

        public List<ILogicElementViewModel> GetAnalogElementsViewModels()
        {
            var analogElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            var analogElementViewModels = new List<ILogicElementViewModel>();
            foreach (var element in analogElements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                analogElementViewModels.Add(viewmodel);
            }

            return analogElementViewModels;
        }

        public List<ILogicElementViewModel> GetAllElementsViewModels(List<ILogicElement> elements)
        {
            var elementsViewModels = new List<ILogicElementViewModel>();
            foreach (var element in elements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                elementsViewModels.Add(viewmodel);
            }

            return elementsViewModels;
        }

        public List<ILogicElementViewModel> GetAllElementsViewModels(List<ILibraryElement> libraryElements)
        {
            var elementsViewModels = new List<ILogicElementViewModel>();

            var allElements = this._container.ResolveAll<ILogicElement>().ToArray();
            var elements = new List<ILogicElement>();

            foreach (var libraryElement in libraryElements)
            {
                var element = allElements.First(logicElement => logicElement.ElementType == libraryElement.ElementType);
                element.CopyLibraryValues(libraryElement);
                elements.Add(element);
            }

            foreach (var element in elements)
            {
                var viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL, 
                    new ResolverParameter("model", element));
                elementsViewModels.Add(viewmodel);
            }

            return elementsViewModels;
        }
    }
}
