using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class BoolFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        private IUshortsFormatter _boolUshortsFormatter;
        private ITypesContainer _container;

        public BoolFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._boolUshortsFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.BOOL_FORMATTER);
        }


        public override IUshortsFormatter GetFormatter()
        {
            return this._boolUshortsFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._boolUshortsFormatter = ushortsFormatter;
        }

        public override string StrongName => StringKeys.BOOL_FORMATTER;

        public override object Model
        {
            get { return this._boolUshortsFormatter; }
            set { this._boolUshortsFormatter = value as IUshortsFormatter; }
        }

        public override object Clone()
        {
            return new BoolFormatterViewModel(this._container);
        }
    }
}