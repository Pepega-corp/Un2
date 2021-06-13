using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
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

        public List<IConnectorViewModel> Inputs =>
            ConnectorViewModels.Where(c => c.Orientation == ConnectorOrientation.LEFT).ToList();
        
        public IConnectorViewModel Output => ConnectorViewModels.First(c => c.Orientation == ConnectorOrientation.RIGHT);
        public int Width => 20;
        public int Height => Inputs.Count * 10 + 10;
    }
}