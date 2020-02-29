using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class DirectFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        public override string StrongName => StringKeys.DIRECT_USHORT_FORMATTER;
        
        public override object Clone()
        {
            DirectFormatterViewModel cloneDirectFormatterViewModel = new DirectFormatterViewModel();
            return cloneDirectFormatterViewModel;
        }
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitDirectUshortFormatter(this);
        }
    }
}
