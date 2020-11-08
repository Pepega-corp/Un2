namespace Unicon2.Presentation.Infrastructure.TreeGrid
{
   public interface IAsTableViewModel
    {
        bool IsTableView { get; set; }
        string AsossiatedDetailsViewName { get; }
    }
}
