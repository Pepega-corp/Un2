using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface IJournal : ILogicElement
    {
        List<string> OutputSignals { get; set; }
        int OutputSignalNum { get; set; }
    }
}