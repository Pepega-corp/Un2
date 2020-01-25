namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ISchemeElement: ISelectable
    {
        double X { get; set; }
        double Y { get; set; }
    }
}