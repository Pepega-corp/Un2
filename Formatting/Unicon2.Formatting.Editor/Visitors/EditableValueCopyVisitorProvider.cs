using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;

namespace Unicon2.Formatting.Editor.Visitors
{
    public class EditableValueCopyVisitorProvider: IEditableValueCopyVisitorProvider
    {
        public IEditableValueViewModelVisitor<Result> GetValueViewModelCopyVisitor(IEditableValueViewModel valueToCopy)
        {
            return new EditableValueCopyVisitor(valueToCopy);
        }
    }
}
