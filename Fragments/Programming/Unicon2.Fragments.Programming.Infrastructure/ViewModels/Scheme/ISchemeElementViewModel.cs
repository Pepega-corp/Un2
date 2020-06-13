namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ISchemeElementViewModel: ISelectable
    {
        double X { get; set; }
        double Y { get; set; }
    }
}