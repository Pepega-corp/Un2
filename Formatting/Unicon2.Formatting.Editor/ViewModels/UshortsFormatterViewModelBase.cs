using Unicon2.Formatting.Editor.ViewModels.Interfaces;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public abstract class UshortsFormatterViewModelBase : ValidatableBindableBase, IUshortsFormatterViewModel, IFormatterViewModelVisitable
    {
        public abstract string StrongName { get; }
        public abstract object Clone();
        public abstract T Accept<T>(IFormatterViewModelVisitor<T> visitor);
    }
}