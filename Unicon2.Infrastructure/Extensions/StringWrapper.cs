using Unicon2.Unity.ViewModels;

namespace Unicon2.Infrastructure.Extensions
{
    public class StringWrapper : ViewModelBase
    {
        public StringWrapper(string value)
        {
            StringValue = value;
        }


        private string _stringValue;

        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value;
                RaisePropertyChanged();
            }
        }
    }

}
