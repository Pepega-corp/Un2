using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
	public class FormatterParametersViewModel : ViewModelBase, IFormatterParametersViewModel
	{
		private IUshortsFormatterViewModel _relatedUshortsFormatterViewModel;
		private bool _isFromSharedResources;
		private string _name;

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

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				RaisePropertyChanged();
			}
		}
	}
}