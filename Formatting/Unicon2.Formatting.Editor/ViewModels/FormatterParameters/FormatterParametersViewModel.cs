using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels.FormatterParameters
{
	public class FormatterParametersViewModel : ViewModelBase, IFormatterParametersViewModel
	{
		private IUshortsFormatterViewModel _relatedUshortsFormatterViewModel;
		private bool _isFromSharedResources;
		private string _name;

		

		public IUshortsFormatterViewModel RelatedUshortsFormatterViewModel
		{
			get => _relatedUshortsFormatterViewModel;
			set
			{
				_relatedUshortsFormatterViewModel = value;
				RaisePropertyChanged();
			}
		}

		public bool IsFromSharedResources
		{
			get => _isFromSharedResources;
			set
			{
				_isFromSharedResources = value;
				RaisePropertyChanged();
			}
		}
		

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged();
			}
		}

		public IFormatterParametersViewModel Clone()
		{
			if (IsFromSharedResources)
			{
				return this;
			}

			return new FormatterParametersViewModel()
			{
				RelatedUshortsFormatterViewModel =
					RelatedUshortsFormatterViewModel.Clone() as IUshortsFormatterViewModel,
				Name = Name,
				IsFromSharedResources = false
			};
		}
	}
}