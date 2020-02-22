using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Values
{
    public interface IFormattedValue : IStronglyNamed
    {
        string Header { get; set; }
        string AsString();
    }
}