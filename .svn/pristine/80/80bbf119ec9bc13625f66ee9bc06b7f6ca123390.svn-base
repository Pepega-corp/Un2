using Unicon2.Unity.ViewModels;

namespace Unicon2.Infrastructure.Extensions
{
    public class StringWrapper : ViewModelBase
    {
        public StringWrapper(string value)
        {
            this.StringValue = value;
        }


        private string _stringValue;

        public string StringValue
        {
            get { return this._stringValue; }
            set
            {
                this._stringValue = value;
                this.RaisePropertyChanged();
            }
        }
    }

}
