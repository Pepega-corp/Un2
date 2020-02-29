using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Editor.ViewModels.Interfaces
{
    public interface IFormatterViewModelVisitable
    {
        T Accept<T>(IFormatterViewModelVisitor<T> visitor);
    }
}