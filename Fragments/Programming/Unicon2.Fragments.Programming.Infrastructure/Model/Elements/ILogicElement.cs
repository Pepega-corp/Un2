using System;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model.Elements
{
    public interface ILogicElement : IStronglyNamed, ICloneable
    {
        Functional Functional { get; }
        Group Group { get; }
        int BinSize { get; }
        ushort[] GetProgrammBin();
        void BinProgrammToProperty(ushort[] bin);
        
        // Все свойства, описывающие данный элемент
        //Dictionary<string, object> Property { get; }
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