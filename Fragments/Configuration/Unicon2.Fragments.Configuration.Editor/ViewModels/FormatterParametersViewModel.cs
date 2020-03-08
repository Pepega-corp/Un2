using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
	public class FormatterParametersViewModel:ViewModelBase, IFormatterParametersViewModel
	{
		private IUshortsFormatterViewModel _relatedUshortsFormatterViewModel;
		private bool _isFromSharedResources;

		public FormatterParametersViewModel()
		{
		}

		public void ShowFormatterParameters()
		{
			throw new NotImplementedException();
		}

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

		public object Clone()
		{
			if (IsFromSharedResources)
			{
				return new FormatterParametersViewModel();
			}r
			return new FormatterParametersViewModel()
			{
				 RelatedUshortsFormatterViewModel = RelatedUshortsFormatterViewModel
			};
		}
	}
}
