using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.Dependencies
{
    public interface IDependencyViewModel : ICloneable<IDependencyViewModel>
    {
        string Name { get; }
    }
}