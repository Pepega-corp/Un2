using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers
{
    public interface IArgumentViewModel :ICloneable<IArgumentViewModel>
    {
        string ArgumentName { get; set; }
        string ResourceNameString { get; set; }
        double TestValue { get; set; }
    }
}