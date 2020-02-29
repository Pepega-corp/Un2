using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class BoolFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {

        public override string StrongName => StringKeys.BOOL_FORMATTER;


        public override object Clone()
        {
            return new BoolFormatterViewModel();
        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitBoolFormatter(this);
        }
    }
}