namespace Unicon2.Infrastructure.Values.Matrix
{
    public interface IMatrixValue : IFormattedValue
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}