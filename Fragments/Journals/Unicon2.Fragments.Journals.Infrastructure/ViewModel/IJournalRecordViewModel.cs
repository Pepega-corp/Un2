using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Journals.Infrastructure.ViewModel
{
    public interface IJournalRecordViewModel : IStronglyNamed
    {
        int NumberOfRecord { get; set; }
        List<IFormattedValueViewModel> FormattedValueViewModels { get; set; }
    }
}