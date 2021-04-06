using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface ILogicElement : IStronglyNamed
    {
        ElementType ElementType { get; }
        string Name { get; }
        Functional Functional { get; }
        Group Group { get; }
        List<IConnector> Connectors { get; set; }
        int BinSize { get; }
        ushort[] GetProgrammBin();
        void BinProgrammToProperty(ushort[] bin);
        void CopyLibraryValues(ILibraryElement source);
        double X { get; set; }
        double Y { get; set; }
    }
}