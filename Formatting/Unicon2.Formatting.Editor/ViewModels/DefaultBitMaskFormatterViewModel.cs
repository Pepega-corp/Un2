using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Unity.Commands;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DefaultBitMaskFormatterViewModel : UshortsFormatterViewModelBase, IBitMaskFormatterViewModel
    {
        private StringWrapper _selectedBitSignature;
        private ObservableCollection<StringWrapper> _bitSignatures;

        public DefaultBitMaskFormatterViewModel()
        {
            BitSignatures = new ObservableCollection<StringWrapper>();
            AddSignatureCommand = new RelayCommand(() =>
              {
                  BitSignatures.Add(new StringWrapper(string.Empty));
              });
            DeleteSignatureCommand = new RelayCommand(() =>
            {
                BitSignatures.Remove(SelectedBitSignature);
            }, () => SelectedBitSignature != null);
        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitBitMaskFormatter(this);
        }

        public override string StrongName => StringKeys.DEFAULT_BIT_MASK_FORMATTER;


        public override object Clone()
        {
            DefaultBitMaskFormatterViewModel clone = new DefaultBitMaskFormatterViewModel();
            clone.BitSignatures = new ObservableCollection<StringWrapper>(BitSignatures.Select(wrapper => new StringWrapper(wrapper.StringValue)));
            return clone;
        }

        public ObservableCollection<StringWrapper> BitSignatures
        {
            get { return _bitSignatures; }
            set
            {
                _bitSignatures = value;
                RaisePropertyChanged();
            }
        }

        public StringWrapper SelectedBitSignature
        {
            get { return _selectedBitSignature; }
            set
            {
                _selectedBitSignature = value;
                RaisePropertyChanged();
                (DeleteSignatureCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddSignatureCommand { get; }
        public ICommand DeleteSignatureCommand { get; }
    }
}
