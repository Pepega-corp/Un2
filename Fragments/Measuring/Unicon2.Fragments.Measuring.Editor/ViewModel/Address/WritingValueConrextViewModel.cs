using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Address
{
    public class WritingValueContextViewModel : ViewModelBase, IWritingValueContextViewModel
    {
        private ushort _address;
        private ushort _numberOfFunction;
        private ushort _valueToWrite;

        public ushort Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }

        public ushort NumberOfFunction
        {
            get { return _numberOfFunction; }
            set
            {
                _numberOfFunction = value;
                RaisePropertyChanged();
            }
        }

        public ushort ValueToWrite
        {
            get { return _valueToWrite; }
            set
            {
                _valueToWrite = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName => MeasuringKeys.WRITING_VALUE_CONTEXT;
     
    }
}
