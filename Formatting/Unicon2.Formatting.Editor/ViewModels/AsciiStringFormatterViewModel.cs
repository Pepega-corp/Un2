using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class AsciiStringFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        public override string StrongName => StringKeys.ASCII_STRING_FORMATTER;

        public override object Clone()
        {
            return new AsciiStringFormatterViewModel();
        }

        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitAsciiStringFormatter(this);
        }
    }
}