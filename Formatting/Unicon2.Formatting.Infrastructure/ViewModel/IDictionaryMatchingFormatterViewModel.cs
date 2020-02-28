using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface IDictionaryMatchingFormatterViewModel : IDynamicFormatterViewModel
    {
        ICommand AddKeyValuePairCommand { get; }
        ICommand ImportFromSharedTablesCommand { get; }

        ICommand DeleteKeyValuePairCommand { get; }
        ObservableCollection<BindableKeyValuePair<ushort, string>> KeyValuesDictionary { get; set; }
        bool IsKeysAreNumbersOfBits { get; set; }

    }
}