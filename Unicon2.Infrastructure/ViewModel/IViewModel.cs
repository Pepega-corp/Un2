using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.ViewModel
{
    public interface IViewModel : IStronglyNamed
    {
        object Model { get; set; }
    }

    public interface IViewModel<T> : IViewModel
    {
        new T Model { get; set; }
    }
}