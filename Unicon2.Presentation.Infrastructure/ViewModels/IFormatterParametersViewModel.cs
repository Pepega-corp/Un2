using System;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
	public interface IFormatterParametersViewModel : INameable,ICloneable<IFormatterParametersViewModel>
	{
		IUshortsFormatterViewModel RelatedUshortsFormatterViewModel { get; set; }
		bool IsFromSharedResources { get; set; }
	}
}