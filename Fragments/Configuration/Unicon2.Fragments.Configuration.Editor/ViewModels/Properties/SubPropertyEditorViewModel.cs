using System;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels.Properties
{
    public class SubPropertyEditorViewModel : PropertyEditorViewModel, ISubPropertyEditorViewModel
    {
        public SubPropertyEditorViewModel(ITypesContainer container, IRangeViewModel rangeViewModel,
            ILocalizerService localizerService) : base(container, rangeViewModel, localizerService)
        {
            BitNumbersInWord = new ObservableCollection<ISharedBitViewModel>();

        }

        public ObservableCollection<ISharedBitViewModel> BitNumbersInWord { get; set; }

        public void SetMainBitNumbersInWord(ObservableCollection<ISharedBitViewModel> mainBitViewModels)
        {
            throw new NotImplementedException();
        }

        public override string StrongName => ConfigurationKeys.SUB_PROPERTY +
                                             ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;


        protected override string GetTypeName()
        {
            return ConfigurationKeys.SUB_PROPERTY;
        }

        public override string Address
        {
            get { return (Parent as PropertyEditorViewModel).Address; }
            set
            {
                if (Parent == null) return;
                (Parent as PropertyEditorViewModel).Address = value;
                RaisePropertyChanged();
            }
        }

        public override T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor)
        {
	        return visitor.VisitSubProperty(this);
        }
		public override string NumberOfPoints
        {
            get { return (Parent as PropertyEditorViewModel)?.NumberOfPoints; }
            set => RaisePropertyChanged();
        }

		public override object Clone()
		{
			var cloneEditorViewModel = new SubPropertyEditorViewModel(_container,
				_rangeViewModel.Clone() as IRangeViewModel, _localizerService)
			{
				Address = Address,
				IsMeasureUnitEnabled = IsMeasureUnitEnabled,
				NumberOfPoints = NumberOfPoints,
				IsRangeEnabled = IsRangeEnabled,
				FormatterParametersViewModel = FormatterParametersViewModel?.Clone() as IFormatterParametersViewModel,
				Header = Header,
				Name = Name,
				MeasureUnit = MeasureUnit,
			};

			return cloneEditorViewModel;
		}
	}
}