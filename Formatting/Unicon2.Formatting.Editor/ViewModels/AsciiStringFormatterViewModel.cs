using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Editor.ViewModels
{
    public class AsciiStringFormatterViewModel : UshortsFormatterViewModelBase, IStaticFormatterViewModel
    {
        public AsciiStringFormatterViewModel()
        {
        }

        public override string StrongName => StringKeys.ASCII_STRING_FORMATTER;

        public override object Clone()
        {
            return new AsciiStringFormatterViewModel();

        }
    }
}