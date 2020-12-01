using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.ViewModel
{
    public class JournalRecordViewModel : ViewModelBase, IJournalRecordViewModel
    {
        private int _numberOfRecord;
        private List<IFormattedValueViewModel> _formattedValueViewModels;

        public JournalRecordViewModel()
        {
            FormattedValueViewModels = new List<IFormattedValueViewModel>();
        }

        public int NumberOfRecord
        {
            get { return _numberOfRecord; }
            set
            {
                _numberOfRecord = value;
                RaisePropertyChanged();
            }
        }

        public List<IFormattedValueViewModel> FormattedValueViewModels
        {
            get { return _formattedValueViewModels; }
            set
            {
                _formattedValueViewModels = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName =>
            JournalKeys.JOURNAL_RECORD + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
      
    }
}
