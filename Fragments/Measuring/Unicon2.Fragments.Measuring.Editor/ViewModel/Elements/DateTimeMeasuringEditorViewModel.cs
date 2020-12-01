using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Elements
{
   public class DateTimeMeasuringEditorViewModel:MeasuringElementEditorViewModelBase, IDateTimeMeasuringEditorViewModel
    {
        private ushort _startAddress;

        public override string StrongName => MeasuringKeys.DATE_TIME_ELEMENT +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public override string NameForUiKey => MeasuringKeys.DATE_TIME_ELEMENT;

        public ushort StartAddress
        {
            get { return this._startAddress; }
            set
            {
                this._startAddress = value;
                RaisePropertyChanged();
            }
        }
    }
}
