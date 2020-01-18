using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public class ControlSignalEditorViewModel : MeasuringElementEditorViewModelBase, IControlSignalEditorViewModel
    {
        private IWritingValueContextViewModel _writingValueContextViewModel;
        public ControlSignalEditorViewModel(IWritingValueContextViewModel writingValueContextViewModel)
        {
            this._writingValueContextViewModel = writingValueContextViewModel;
        }


        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        protected override IMeasuringElement GetModel()
        {
            (this._measuringElement as IControlSignal).WritingValueContext = this._writingValueContextViewModel.Model as IWritingValueContext;
            return base.GetModel();
        }


        protected override void SetModel(object value)
        {
            base.SetModel(value);
            this._writingValueContextViewModel.Model = (this._measuringElement as IControlSignal).WritingValueContext;
        }


        public override string NameForUiKey => MeasuringKeys.CONTROL_SIGNAL;

        public IWritingValueContextViewModel WritingValueContextViewModel
        {
            get { return this._writingValueContextViewModel; }
            set
            {
                this._writingValueContextViewModel = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
