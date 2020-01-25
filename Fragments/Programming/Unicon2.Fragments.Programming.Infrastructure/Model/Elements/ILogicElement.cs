using System;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface ILogicElement : IStronglyNamed, ICloneable
    {
        string Name { get; set; }
        Functional Functional { get; }
        Group Group { get; }
        int BinSize { get; }
        ushort[] GetProgrammBin();
        void BinProgrammToProperty(ushort[] bin);
        void CopyValues(ILogicElement source);
    }

    public enum Functional
    {
        BOOLEAN,
        ANALOG
    }

    public enum Group
    {
        INPUT_OUTPUT,
        SIMPLE_LOGIC,
        EXTENDET_LOGIC,
        OTHER
    }
}