using Unicon2.Formatting.Editor.Visitors;

namespace Unicon2.Formatting.Editor.ViewModels.Interfaces
{
    public interface IFormatterViewModelVisitable
    {
        T Accept<T>(IFormatterViewModelVisitor<T> visitor);
    }
}