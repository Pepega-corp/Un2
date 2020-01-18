using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Factories;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Journals.Editor.ViewModel.JournalParameters
{
    public class SubJournalParameterEditorViewModel : JournalParameterEditorViewModel, ISubJournalParameterEditorViewModel
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        public SubJournalParameterEditorViewModel(ISubJournalParameter journalParameter, IFormatterEditorFactory formatterEditorFactory) : base(journalParameter)
        {
            this._formatterEditorFactory = formatterEditorFactory;
            this.BitNumbersInWord = new ObservableCollection<ISharedBitViewModel>();
            this.ShowFormatterParametersCommand = new RelayCommand(this.OnShowFormatterParametersExecute);
        }

        private void OnShowFormatterParametersExecute()
        {
            this._formatterEditorFactory.EditFormatterByUser(this._journalParameter);
            this.RaisePropertyChanged(nameof(this.FormatterString));
        }

        public ObservableCollection<ISharedBitViewModel> BitNumbersInWord { get; set; }
        public ICommand ShowFormatterParametersCommand { get; }

        protected override void SetModel(object value)
        {
            if (value is ISubJournalParameter)
            {
                foreach (int bitNum in (value as ISubJournalParameter).BitNumbersInWord)
                {

                    ISharedBitViewModel sharedBit = this.BitNumbersInWord.First(model => model.NumberOfBit == bitNum);
                    if (sharedBit.Value) sharedBit.Refresh();

                    this.BitNumbersInWord.First((viewModel => viewModel.NumberOfBit == bitNum)).ChangeValueByOwnerCommand?.Execute(this);
                }
            }
            base.SetModel(value);
        }



        protected override void SaveModel()
        {
            ISubJournalParameter subJournalParameter = this._journalParameter as ISubJournalParameter;
            subJournalParameter.BitNumbersInWord.Clear();
            foreach (ISharedBitViewModel sharedBitViewModel in this.BitNumbersInWord)
            {
                if ((sharedBitViewModel.Owner == this) && (sharedBitViewModel.Value))
                    subJournalParameter.BitNumbersInWord.Add(sharedBitViewModel.NumberOfBit);
            }
            base.SaveModel();
        }
    }
}
