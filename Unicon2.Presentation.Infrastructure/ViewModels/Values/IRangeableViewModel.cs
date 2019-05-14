namespace Unicon2.Presentation.Infrastructure.ViewModels.Values
{
    public interface IRangeableViewModel
    {
        bool IsRangeEnabled { get; set; }
        IRangeViewModel RangeViewModel { get; set; }
    }
}