using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Base;

namespace Unicon2.Fragments.Configuration.Editor.ViewModels
{
    public abstract class EditorConfigurationItemViewModelBase : ConfigurationItemViewModelBase,
        IEditorConfigurationItemViewModel
    {
        public abstract override string TypeName { get; }

        public abstract object Clone();

        public string Name
        {
            get => Header;
            set => Header = value;
        }

        public abstract T Accept<T>(IConfigurationItemViewModelVisitor<T> visitor);
    }
}