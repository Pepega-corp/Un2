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
            this.BitSignatures = new ObservableCollection<StringWrapper>();
            this.AddSignatureCommand = new RelayCommand(() =>
              {
                  this.BitSignatures.Add(new StringWrapper(string.Empty));
              });
            this.DeleteSignatureCommand = new RelayCommand(() =>
            {
                this.BitSignatures.Remove(this.SelectedBitSignature);
            }, () => this.SelectedBitSignature != null);
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
            get { return this._bitSignatures; }
            set
            {
                this._bitSignatures = value;
                this.RaisePropertyChanged();
            }
        }

        public StringWrapper SelectedBitSignature
        {
            get { return this._selectedBitSignature; }
            set
            {
                this._selectedBitSignature = value;
                this.RaisePropertyChanged();
                (this.DeleteSignatureCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddSignatureCommand { get; }
        public ICommand DeleteSignatureCommand { get; }
    }
}
