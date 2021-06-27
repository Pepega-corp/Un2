using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Presentation.Infrastructure.Extensions
{
    public static class PresentationExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        public static void CopyBitsTo(this IBitsConfigViewModel configViewModelSource,
            IBitsConfigViewModel configViewModelTarget)
        {
            configViewModelTarget.IsFromBits = configViewModelSource.IsFromBits;
            for (int i = 15; i >= 0; i--)
            {
                configViewModelTarget.BitNumbersInWord[i].IsChecked =
                    configViewModelSource.BitNumbersInWord[i].IsChecked;
            }
        }


        public static string BuildItemPath(this IConfigurationItemViewModel configurationViewModel)
        {
            return (configurationViewModel.Parent != null
                ? $"{BuildItemPath(configurationViewModel.Parent)}."
                : null) + configurationViewModel.Header;
        }
    }
}