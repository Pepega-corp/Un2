using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class UshortToIntegerFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        private readonly ITypesContainer _container;
        private IUshortsFormatter _ushortToIntegerFormatter;

        public UshortToIntegerFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._ushortToIntegerFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.USHORT_TO_INTEGER_FORMATTER);
        }

        public override string StrongName => this._ushortToIntegerFormatter.StrongName;

        public override IUshortsFormatter GetFormatter()
        {
            return this._ushortToIntegerFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._ushortToIntegerFormatter = ushortsFormatter ?? this._container.Resolve<IUshortsFormatter>(StringKeys.ASCII_STRING_FORMATTER);
        }


        public override object Model
        {
            get => this._ushortToIntegerFormatter;
            set
            {
                if (value is IUshortsFormatter)
                    this._ushortToIntegerFormatter = value as IUshortsFormatter;
            }
        }

        public override object Clone()
        {
            return new AsciiStringFormatterViewModel(this._container);

        }
    }
}