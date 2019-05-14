namespace Unicon2.Infrastructure.Interfaces
{
    public interface IRangeable
    {
        bool IsRangeEnabled { get; set; }
        IRange Range { get; set; }
    }
}