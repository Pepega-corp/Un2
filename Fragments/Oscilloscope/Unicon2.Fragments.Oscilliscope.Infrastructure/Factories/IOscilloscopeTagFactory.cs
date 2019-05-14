using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.Factories
{
    public interface IOscilloscopeTagFactory
    {
        IOscilloscopeTag CreateOscilloscopeTag(string key);
    }
}
