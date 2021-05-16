using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface ISchemeModel
    {
        string SchemeName { get; set; }
        double SchemeHeight { get; }
        double SchemeWidth { get; }
        List<ILogicElement> LogicElements {get; set; }
        List<int> ConnectionNumbers { get; set; }
    }
}
