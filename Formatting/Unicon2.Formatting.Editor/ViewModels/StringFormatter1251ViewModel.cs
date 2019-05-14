using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    internal class StringFormatter1251ViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        private readonly ITypesContainer _container;
        private IUshortsFormatter _stringFormatter1251;

        public StringFormatter1251ViewModel(ITypesContainer container)
        {
            this._container = container;
            this._stringFormatter1251 = this._container.Resolve<IUshortsFormatter>(StringKeys.STRING_FORMATTER1251);
        }

        public override string StrongName => this._stringFormatter1251.StrongName;

        public override IUshortsFormatter GetFormatter()
        {
            return this._stringFormatter1251;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._stringFormatter1251 = ushortsFormatter ?? this._container.Resolve<IUshortsFormatter>(StringKeys.STRING_FORMATTER1251);
        }


        public override object Model
        {
            get => this._stringFormatter1251;
            set
            {
                if (value is IUshortsFormatter)
                    this._stringFormatter1251 = value as IUshortsFormatter;
            }
        }

        public override object Clone()
        {
            return new AsciiStringFormatterViewModel(this._container);

        }
    }
}