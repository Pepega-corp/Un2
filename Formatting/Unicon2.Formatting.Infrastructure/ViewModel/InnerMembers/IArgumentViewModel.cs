using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers
{
    public interface IArgumentViewModel 
    {
        string ArgumentName { get; set; }
        string ResourceNameString { get; set; }
        double TestValue { get; set; }
    }
}