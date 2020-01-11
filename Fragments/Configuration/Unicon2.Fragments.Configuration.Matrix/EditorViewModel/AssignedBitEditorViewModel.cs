using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class AssignedBitEditorViewModel : ViewModelBase, IAssignedBitEditorViewModel
    {
        private IBitOptionEditorViewModel _selectedBitOptionEditorViewModel;
        private ObservableCollection<IBitOptionEditorViewModel> _bitOptionEditorViewModels;
        private bool _isBitAssigned;

        public AssignedBitEditorViewModel()
        {
            this.ResetCommand = new RelayCommand(this.OnResetExecute);
        }

        private void OnResetExecute()
        {
            this.SelectedBitOptionEditorViewModel = null;
        }


        public int NumberOfBit { get; set; }

        public IBitOptionEditorViewModel SelectedBitOptionEditorViewModel
        {
            get { return this._selectedBitOptionEditorViewModel; }
            set
            {
                IBitOptionEditorViewModel prevBitOptionEditorViewModel = this._selectedBitOptionEditorViewModel;

                if (prevBitOptionEditorViewModel == value) return;

                if (prevBitOptionEditorViewModel != null &&
                    prevBitOptionEditorViewModel.NumbersOfAssotiatedBits.Contains(this.NumberOfBit))
                {
                    prevBitOptionEditorViewModel.NumbersOfAssotiatedBits.Remove(this.NumberOfBit);
                }


                this._selectedBitOptionEditorViewModel = value;
                if (this._selectedBitOptionEditorViewModel != null)
                {
                    if (!this._selectedBitOptionEditorViewModel.NumbersOfAssotiatedBits.Contains(this.NumberOfBit))
                    {
                        this._selectedBitOptionEditorViewModel.NumbersOfAssotiatedBits.Add(this.NumberOfBit);
                    }
                    this.IsBitAssigned = true;
                }
                else
                {
                    this.IsBitAssigned = false;
                }

                this.BitOptionEditorViewModels?.ForEach(model => model.UpdateIsEnabled());
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels
        {
            get { return this._bitOptionEditorViewModels; }
            set
            {
                this._bitOptionEditorViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsBitAssigned
        {
            get { return this._isBitAssigned; }
            set
            {
                this._isBitAssigned = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand ResetCommand { get; }

        public string StrongName => nameof(AssignedBitEditorViewModel);

        public object Model { get; set; }
    }
}
