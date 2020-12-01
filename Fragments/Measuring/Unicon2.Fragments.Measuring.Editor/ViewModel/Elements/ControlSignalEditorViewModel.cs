using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
    public class ControlSignalEditorViewModel : MeasuringElementEditorViewModelBase, IControlSignalEditorViewModel
    {
        private IWritingValueContextViewModel _writingValueContextViewModel;
        public ControlSignalEditorViewModel(IWritingValueContextViewModel writingValueContextViewModel)
        {
            _writingValueContextViewModel = writingValueContextViewModel;
        }


        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public override string NameForUiKey => MeasuringKeys.CONTROL_SIGNAL;

        public IWritingValueContextViewModel WritingValueContextViewModel
        {
            get { return _writingValueContextViewModel; }
            set
            {
                _writingValueContextViewModel = value;
                RaisePropertyChanged();
            }
        }
    }
}
