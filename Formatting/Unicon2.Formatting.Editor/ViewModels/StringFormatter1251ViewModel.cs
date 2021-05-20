using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class StringFormatter1251ViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        public override string StrongName => StringKeys.STRING_FORMATTER1251;

        public override object Clone()
        {
            return new StringFormatter1251ViewModel();
        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitString1251Formatter(this);
        }
    }
}