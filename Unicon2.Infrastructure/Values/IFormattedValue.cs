using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Infrastructure.Values
{
    public interface IFormattedValue : IStronglyNamed
    {
        string Header { get; set; }
        string AsString();
        T Accept<T>(IValueVisitor<T> visitor);
    }
}