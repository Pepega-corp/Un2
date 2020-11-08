using System.Windows;
using System.Windows.Controls;
using Unicon2.Fragments.Measuring.Editor.ViewModel.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Editor.ContentTemplateSelectors
{
	public class SelectedPresentationElementTemplateSelector:DataTemplateSelector
	{
		public DataTemplate TemplateForGroup { get; set; }
		public DataTemplate TemplateForOthers { get; set; }


		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is PresentationElementViewModel presentationElementViewModel &&
			    presentationElementViewModel.TemplatedViewModelToShowOnCanvas is PresentationGroupViewModel
				    presentationGroupViewModel)
			{
				return TemplateForGroup;
			}
			return TemplateForOthers;
		}
	}
}
