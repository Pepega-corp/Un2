using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class StringValueViewModel : EditableValueViewModelBase, IStringValueViewModel
    {
        private string _stringValue;

        public override string StrongName => nameof(StringValueViewModel);

        public string StringValue
        {
            get { return _stringValue; }
            set
            {
                _stringValue = value;
                RaisePropertyChanged();
            }
        } 
        public override T Accept<T>(IEditableValueViewModelVisitor<T> visitor)
        {
            return visitor.VisitStringValueViewModel(this);
        }
    }
}