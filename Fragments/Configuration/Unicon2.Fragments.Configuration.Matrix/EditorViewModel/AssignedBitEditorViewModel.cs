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
            ResetCommand = new RelayCommand(OnResetExecute);
        }

        private void OnResetExecute()
        {
            SelectedBitOptionEditorViewModel = null;
        }


        public int NumberOfBit { get; set; }

        public IBitOptionEditorViewModel SelectedBitOptionEditorViewModel
        {
            get { return _selectedBitOptionEditorViewModel; }
            set
            {
                IBitOptionEditorViewModel prevBitOptionEditorViewModel = _selectedBitOptionEditorViewModel;

                if (prevBitOptionEditorViewModel == value) return;

                if (prevBitOptionEditorViewModel != null &&
                    prevBitOptionEditorViewModel.NumbersOfAssotiatedBits.Contains(NumberOfBit))
                {
                    prevBitOptionEditorViewModel.NumbersOfAssotiatedBits.Remove(NumberOfBit);
                }


                _selectedBitOptionEditorViewModel = value;
                if (_selectedBitOptionEditorViewModel != null)
                {
                    if (!_selectedBitOptionEditorViewModel.NumbersOfAssotiatedBits.Contains(NumberOfBit))
                    {
                        _selectedBitOptionEditorViewModel.NumbersOfAssotiatedBits.Add(NumberOfBit);
                    }
                    IsBitAssigned = true;
                }
                else
                {
                    IsBitAssigned = false;
                }

                BitOptionEditorViewModels?.ForEach(model => model.UpdateIsEnabled());
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels
        {
            get { return _bitOptionEditorViewModels; }
            set
            {
                _bitOptionEditorViewModels = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBitAssigned
        {
            get { return _isBitAssigned; }
            set
            {
                _isBitAssigned = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ResetCommand { get; }

        public string StrongName => nameof(AssignedBitEditorViewModel);

        public object Model { get; set; }
    }
}
