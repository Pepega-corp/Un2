using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Factories
{
    public class LogicElementFactory : ILogicElementFactory
    {
        private ITypesContainer _container;

        public LogicElementFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public List<ILogicElementEditorViewModel> GetBooleanElementsEditorViewModels()
        {
            List<ILogicElement> booleanElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();
            List<ILogicElementEditorViewModel> booleanElementViewModels = new List<ILogicElementEditorViewModel>();
            foreach (ILogicElement element in booleanElements)
            {
                ILogicElementEditorViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
                viewmodel.Model = element;
                booleanElementViewModels.Add(viewmodel);
            }

            return booleanElementViewModels;
        }

        public List<ILogicElementEditorViewModel> GetAnalogElementsEditorViewModels()
        {
            List<ILogicElement> analogElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            List<ILogicElementEditorViewModel> analogElementViewModels = new List<ILogicElementEditorViewModel>();
            foreach (ILogicElement element in analogElements)
            {
                ILogicElementEditorViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName +ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
                viewmodel.Model = element;
                analogElementViewModels.Add(viewmodel);
            }

            return analogElementViewModels;
        }

        public List<ILogicElementEditorViewModel> GetAllElementsEditorViewModels(List<ILogicElement> elements)
        {
            List<ILogicElementEditorViewModel> elementsViewModels = new List<ILogicElementEditorViewModel>();
            foreach (ILogicElement element in elements)
            {
                ILogicElementEditorViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementEditorViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
                viewmodel.Model = element;
                elementsViewModels.Add(viewmodel);
            }

            return elementsViewModels;
        }


        public List<ILogicElementViewModel> GetBooleanElementsViewModels()
        {
            List<ILogicElement> booleanElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.BOOLEAN).ToList();
            List<ILogicElementViewModel> booleanElementViewModels = new List<ILogicElementViewModel>();
            foreach (ILogicElement element in booleanElements)
            {
                ILogicElementViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                booleanElementViewModels.Add(viewmodel);
            }

            return booleanElementViewModels;
        }

        public List<ILogicElementViewModel> GetAnalogElementsViewModels()
        {
            List<ILogicElement> analogElements = this._container.ResolveAll<ILogicElement>()
                .Where(e => e.Functional == Functional.ANALOG).ToList();
            List<ILogicElementViewModel> analogElementViewModels = new List<ILogicElementViewModel>();
            foreach (ILogicElement element in analogElements)
            {
                ILogicElementViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                analogElementViewModels.Add(viewmodel);
            }

            return analogElementViewModels;
        }

        public List<ILogicElementViewModel> GetAllElementsViewModels(List<ILogicElement> elements)
        {
            List<ILogicElementViewModel> elementsViewModels = new List<ILogicElementViewModel>();
            foreach (ILogicElement element in elements)
            {
                ILogicElementViewModel viewmodel = StaticContainer.Container.Resolve<ILogicElementViewModel>(
                    element.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
                viewmodel.Model = element;
                elementsViewModels.Add(viewmodel);
            }

            return elementsViewModels;
        }
    }
}
