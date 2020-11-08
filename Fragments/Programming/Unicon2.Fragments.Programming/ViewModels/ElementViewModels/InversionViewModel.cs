using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class InversionViewModel: LogicElementViewModel
    {
        public override string StrongName => ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public InversionViewModel()
        {
            _model = new Inversion();

            this.ElementName = "НЕ";
            this.Description = "Елемент инверсии логического сигнала";
            this.Symbol = "~";
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
        }

        public InversionViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
        }
        
        public override ILogicElementViewModel Clone()
        {
            return (InversionViewModel)Clone<InversionViewModel, Inversion>();
        }
    }
}
