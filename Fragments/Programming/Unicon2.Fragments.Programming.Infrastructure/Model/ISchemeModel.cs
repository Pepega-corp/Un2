using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface ISchemeModel
    {
        string SchemeName { get; set; }
        double SchemeHeight { get; }
        double SchemeWidth { get; }
        double Scale { get; set; }
        ILogicElement[] LogicElements {get; set; }
        int[] ConnectionNumbers { get; set; }
    }
}
