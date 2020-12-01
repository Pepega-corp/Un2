using System.Collections.Generic;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements
{
    public interface IInputEditor : ILibraryElement
    {
        List<Dictionary<int, string>> AllInputSignals { get; set; }
        List<string> Bases { get; set; }
    }
}
