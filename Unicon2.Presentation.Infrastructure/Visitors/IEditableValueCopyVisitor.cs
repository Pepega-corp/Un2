using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Presentation.Infrastructure.Visitors
{
    public interface IEditableValueCopyVisitorProvider
    {
        IEditableValueViewModelVisitor<Result> GetValueViewModelCopyVisitor(IEditableValueViewModel valueToCopy);
    }
    public interface IEditableValueCopyVisitor : IEditableValueViewModelVisitor<Result>
    {

    }
}