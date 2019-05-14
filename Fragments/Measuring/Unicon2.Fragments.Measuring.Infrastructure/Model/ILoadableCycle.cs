using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model
{
    public interface ILoadableCycle : ILoadable
    {
        void StartLoadingCycle( );
        void StopLoadingCycle( );
        bool IsLoadingCycleInProcess { get; }

    }
}