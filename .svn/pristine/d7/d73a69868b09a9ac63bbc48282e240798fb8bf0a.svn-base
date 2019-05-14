using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class AsciiStringFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        private readonly ITypesContainer _container;
        private IUshortsFormatter _asciiStringFormatter;

        public AsciiStringFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._asciiStringFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.ASCII_STRING_FORMATTER);
        }

        public override string StrongName => this._asciiStringFormatter.StrongName;

        public override IUshortsFormatter GetFormatter()
        {
            return this._asciiStringFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._asciiStringFormatter = ushortsFormatter ?? this._container.Resolve<IUshortsFormatter>(StringKeys.ASCII_STRING_FORMATTER);
        }


        public override object Model
        {
            get => this._asciiStringFormatter;
            set
            {
                if (value is IUshortsFormatter)
                    this._asciiStringFormatter = value as IUshortsFormatter;
            }
        }

        public override object Clone()
        {
            return new AsciiStringFormatterViewModel(this._container);

        }
    }
}