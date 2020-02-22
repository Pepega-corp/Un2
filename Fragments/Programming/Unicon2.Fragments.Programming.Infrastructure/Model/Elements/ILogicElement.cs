using System;
using Unicon2.Fragments.Programming.Infrastructure.Enums;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface ILogicElement : IStronglyNamed
    {
        string Name { get; set; }
        Functional Functional { get; }
        Group Group { get; }
        IConnector[] Connectors { get; set; }
        int BinSize { get; }
        ushort[] GetProgrammBin();
        void BinProgrammToProperty(ushort[] bin);
        void CopyValues(ILogicElement source);
        double X { get; set; }
        double Y { get; set; }
    }
}