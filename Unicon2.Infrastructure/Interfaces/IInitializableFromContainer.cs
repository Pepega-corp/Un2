using Unicon2.Unity.Interfaces;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IInitializableFromContainer
    {
        bool IsInitialized { get; }
        void InitializeFromContainer(ITypesContainer container);
    }
}