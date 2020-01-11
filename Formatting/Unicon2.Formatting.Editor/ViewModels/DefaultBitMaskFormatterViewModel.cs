using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DefaultBitMaskFormatterViewModel : UshortsFormatterViewModelBase, IBitMaskFormatterViewModel
    {
        private readonly ITypesContainer _container;
        private IBitMaskFormatter _bitMaskFormatter;
        private StringWrapper _selectedBitSignature;
        private ObservableCollection<StringWrapper> _bitSignatures;

        public DefaultBitMaskFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._bitMaskFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.DEFAULT_BIT_MASK_FORMATTER) as IBitMaskFormatter;

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

        public override IUshortsFormatter GetFormatter()
        {
            this._bitMaskFormatter.BitSignatures = this.BitSignatures.Select(wrapper => wrapper.StringValue).ToList();
            return this._bitMaskFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            if (ushortsFormatter == null)
            {
                this.BitSignatures = new ObservableCollection<StringWrapper>();
            }
            this._bitMaskFormatter = ushortsFormatter as IBitMaskFormatter;
            this.BitSignatures = new ObservableCollection<StringWrapper>();
            this._bitMaskFormatter?.BitSignatures?.ForEach(s => this.BitSignatures.Add(new StringWrapper(s)));
        }

        public override string StrongName => StringKeys.DEFAULT_BIT_MASK_FORMATTER;

        public override object Model
        {
            get { return this._bitMaskFormatter; }
            set
            {
                this._bitMaskFormatter = value as IBitMaskFormatter;
            }
        }

        public override object Clone()
        {
            DefaultBitMaskFormatterViewModel clone = new DefaultBitMaskFormatterViewModel(this._container);
            clone.InitFromFormatter(this._bitMaskFormatter.Clone() as IBitMaskFormatter);
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
