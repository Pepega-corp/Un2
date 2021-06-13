using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class TriggerViewModel: LogicElementViewModel
    {
        public TriggerViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            base._globalCommands = globalCommands;
            _logicElementModel = model;
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            SetModel(_logicElementModel);
        }
    }
}