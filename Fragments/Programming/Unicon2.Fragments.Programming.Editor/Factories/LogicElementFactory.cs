using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
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

        public List<ILogicElementEditorViewModel> GetBooleanElementsViewModels()
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

        public List<ILogicElementEditorViewModel> GetAnalogElementsViewModels()
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

        public List<ILogicElementEditorViewModel> GetAllElementsViewModels(List<ILogicElement> elements)
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
    }
}
