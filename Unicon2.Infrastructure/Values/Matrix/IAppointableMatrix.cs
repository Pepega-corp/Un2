namespace Unicon2.Infrastructure.Values.Matrix
{
    public interface IAppointableMatrix: IProperty
    {
        IMatrixTemplate MatrixTemplate { get; set; }
    }
}