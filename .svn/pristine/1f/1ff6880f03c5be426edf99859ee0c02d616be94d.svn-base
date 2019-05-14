using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DirectFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        private readonly ITypesContainer _container;

        private IUshortsFormatter _directUshortFormatter;

        public DirectFormatterViewModel(ITypesContainer container)
        {
            this._container = container;
            this._directUshortFormatter = this._container.Resolve<IUshortsFormatter>(StringKeys.DIRECT_USHORT_FORMATTER);
        }
        
        public override IUshortsFormatter GetFormatter()
        {
            return this._directUshortFormatter;
        }

        public override void InitFromFormatter(IUshortsFormatter ushortsFormatter)
        {
            this._directUshortFormatter = ushortsFormatter ?? this._container.Resolve<IUshortsFormatter>(StringKeys.DIRECT_USHORT_FORMATTER);
        }

        public override string StrongName => this._directUshortFormatter?.StrongName;

        public override object Model
        {
            get => this._directUshortFormatter;
            set => this._directUshortFormatter = value as IUshortsFormatter;
        }

        public override object Clone()
        {
            DirectFormatterViewModel cloneDirectFormatterViewModel = new DirectFormatterViewModel(this._container);
            cloneDirectFormatterViewModel.InitFromFormatter(this._directUshortFormatter.Clone() as IUshortsFormatter);
            return cloneDirectFormatterViewModel;
        }
    }
}
