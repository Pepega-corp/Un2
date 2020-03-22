using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IControlSignal:IMeasuringElement
    {
        IWritingValueContext WritingValueContext { get; set; }
    }
}