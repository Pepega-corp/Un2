using System;
using System.Windows.Input;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
	public interface IFormatterParametersViewModel: ICloneable
	{
		void ShowFormatterParameters();
		IUshortsFormatterViewModel RelatedUshortsFormatterViewModel { get; set; }
		bool IsFromSharedResources { get; set; }

	}
}