using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public abstract class UshortsFormatterViewModelBase : ValidatableBindableBase, IUshortsFormatterViewModel, IStronglyNamed
    {
        public abstract string StrongName { get; }
        public abstract object Clone();
    }
}