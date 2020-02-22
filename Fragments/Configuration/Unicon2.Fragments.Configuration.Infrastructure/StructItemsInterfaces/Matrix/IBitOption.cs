using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{
    public interface IBitOption:IStronglyNamed
    {
        string FullSignature { get; }
        IVariableColumnSignature VariableColumnSignature { get; set; }
        List<int> NumbersOfAssotiatedBits { get; set; }

        bool IsBitOptionEqual(IBitOption comparingBitOption);
    }
}