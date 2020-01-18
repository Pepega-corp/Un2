using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public abstract class EditorConfigurationItemViewModelBase : ConfigurationItemViewModelBase, IEditorConfigurationItemViewModel
    {
        private IConfigurationItemViewModel _parent;

        public abstract override string TypeName { get; }

        public abstract object Clone();
    }
}
