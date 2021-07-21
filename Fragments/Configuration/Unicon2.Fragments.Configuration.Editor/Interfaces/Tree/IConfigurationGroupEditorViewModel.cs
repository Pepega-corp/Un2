using System.Collections.ObjectModel;
using Unicon2.Fragments.Configuration.Editor.Interfaces.EditOperations;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Filter;
using Unicon2.Fragments.Configuration.Editor.ViewModels;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Interfaces.Tree
{
	public interface IConfigurationGroupEditorViewModel : IEditorConfigurationItemViewModel, IItemGroupViewModel,
		IAddressChangeable, ICompositeEditOperations, IChildPositionChangeable, IChildItemRemovable, ICanBeHidden,
		IAsChildPasteable
	{
		bool IsGroupWithReiteration { get; set; }
		
		ObservableCollection<StringWrapper> SubGroupNames { get; }
		int ReiterationStep { get; set; }

		void SetIsGroupWithReiteration(bool value);
		ObservableCollection<IFilterViewModel> FilterViewModels { get; }
	}
}