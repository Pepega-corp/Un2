using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IControlSignal : IMeasuringElement
    {
        IWritingValueContext WritingValueContext { get; set; }
    }
}