using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Editor.Factories
{
    public class LogicElementFactory : ILogicElementFactory
    {
        private ITypesContainer _container;

        public LogicElementFactory(ITypesContainer container)
        {
            this._container = container;
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
