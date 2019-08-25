using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IOutput : ILogicElement
    {
        List<string> OutputSignals { get; set; }
        int OutputSignalNum { get; set; }
        int ConnectionNumber { get; set; }
    }
}