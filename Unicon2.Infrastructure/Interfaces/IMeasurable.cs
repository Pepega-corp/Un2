namespace Unicon2.Infrastructure.Interfaces
{
    public interface IMeasurable
    {
        string MeasureUnit { get; set; }
        bool IsMeasureUnitEnabled { get; set; }
    }
}