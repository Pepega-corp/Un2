using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IInput : ILogicElement
    {
        List<Dictionary<int, string>> AllInputSignals { get; }
        List<string> Bases { get; }
        int ConnectionNumber { get; set; }
    }
}