using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.Services
{
    public class SaveFormatterService: ISaveFormatterService
    {
        public IUshortsFormatter CreateUshortsFormatter(IUshortsFormatterViewModel ushortsFormatterViewModel)
        {
            if (ushortsFormatterViewModel is UshortsFormatterViewModelBase ushortsFormatterViewModelBase)
            {
                return ushortsFormatterViewModelBase.Accept(new SaveFormatterViewModelVisitor());
            }
            return null;
        }
    }
}