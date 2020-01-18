using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Address
{
    public class WritingValueContextViewModel : ViewModelBase, IWritingValueContextViewModel
    {
        private ushort _address;
        private ushort _numberOfFunction;
        private ushort _valueToWrite;
        private IWritingValueContext _writingValueContext;

        public WritingValueContextViewModel(IWritingValueContext writingValueContext)
        {
            this._writingValueContext = writingValueContext;
        }

        public ushort Address
        {
            get { return this._address; }
            set
            {
                this._address = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort NumberOfFunction
        {
            get { return this._numberOfFunction; }
            set
            {
                this._numberOfFunction = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort ValueToWrite
        {
            get { return this._valueToWrite; }
            set
            {
                this._valueToWrite = value;
                this.RaisePropertyChanged();
            }
        }

        public string StrongName => MeasuringKeys.WRITING_VALUE_CONTEXT;

        public object Model
        {
            get
            {
                this._writingValueContext.Address = this.Address;
                this._writingValueContext.NumberOfFunction = this.NumberOfFunction;
                this._writingValueContext.ValueToWrite = this.ValueToWrite;
                return this._writingValueContext;

            }
            set
            {
                this._writingValueContext = value as IWritingValueContext;
                this.ValueToWrite = this._writingValueContext.ValueToWrite;
                this.NumberOfFunction = this._writingValueContext.NumberOfFunction;
                this.Address = this._writingValueContext.Address;
            }
        }
    }
}
