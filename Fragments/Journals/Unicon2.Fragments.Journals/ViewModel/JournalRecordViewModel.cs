using System;
using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.ViewModel
{
    public class JournalRecordViewModel : ViewModelBase, IJournalRecordViewModel
    {
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private int _numberOfRecord;
        private List<IFormattedValueViewModel> _formattedValueViewModels;
        private IJournalRecord _journalRecord;

        public JournalRecordViewModel(IValueViewModelFactory valueViewModelFactory)
        {
            this._valueViewModelFactory = valueViewModelFactory;
            this.FormattedValueViewModels = new List<IFormattedValueViewModel>();
        }

        public int NumberOfRecord
        {
            get { return this._numberOfRecord; }
            set
            {
                this._numberOfRecord = value;
                this.RaisePropertyChanged();
            }
        }

        public List<IFormattedValueViewModel> FormattedValueViewModels
        {
            get { return this._formattedValueViewModels; }
            set
            {
                this._formattedValueViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public string StrongName => JournalKeys.JOURNAL_RECORD + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public object Model
        {
            get
            {
                throw new NotImplementedException();

            }
            set
            {
                this._journalRecord = value as IJournalRecord;
                this.FormattedValueViewModels.Clear();
                if (this._journalRecord != null)
                {
                    foreach (Unicon2.Infrastructure.Values.IFormattedValue formattedValue in this._journalRecord.FormattedValues)
                    {
                        this.FormattedValueViewModels.Add(this._valueViewModelFactory.CreateFormattedValueViewModel(formattedValue));
                    }
                }
                this.RaisePropertyChanged(nameof(this.FormattedValueViewModels));

            }
        }
    }
}
