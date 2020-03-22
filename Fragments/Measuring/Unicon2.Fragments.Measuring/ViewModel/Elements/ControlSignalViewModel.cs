using System.Windows.Input;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Measuring.ViewModel.Elements
{
    public class ControlSignalViewModel : MeasuringElementViewModelBase, IControlSignalViewModel
    {
        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public ICommand WriteValueCommand { get; set; }


        //protected override void SetModel(object model)
        //{
        //    this.WriteValueCommand = new RelayCommand((() =>
        //     {
        //         (model as IControlSignal).Write();
        //     }));
        //    base.SetModel(model);
        //}
    }
}
