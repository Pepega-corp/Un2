using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements
{
    public interface IOutputEditor : ILibraryElement
    {
        List<string> OutputSignals { get; set; }
    }
}