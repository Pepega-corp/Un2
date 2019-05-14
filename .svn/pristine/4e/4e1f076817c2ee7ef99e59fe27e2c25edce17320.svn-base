using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Editor.Interfaces;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class ElementLibraryViewModel : ViewModelBase, IElementLibraryViewModel
    {
        private IElementLibraryModel _elementLibraryModel;
        private ILogicElementFactory _logicElemFactory;

        public ElementLibraryViewModel(ILogicElementFactory logicElemFactory)
        {
            this._logicElemFactory = logicElemFactory;
            //TODO получить из модели эелементы и посредством фабрики сделать вьюмодели и заполнить коллекцию AllElements
            this.AllElements = new ObservableCollection<ILogicElementViewModel>();
        }


        public ObservableCollection<ILogicElementViewModel> AllElements { get; }

        public string StrongName => ProgrammingKeys.ELEMENT_LIBRARY +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public object Model
        {
            get { return this._elementLibraryModel; }
            set { this._elementLibraryModel = value as IElementLibraryModel; }
        }
    }
}
