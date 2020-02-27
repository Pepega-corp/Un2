using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.Values.Matrix
{
    public interface IBitOption:IStronglyNamed
    {
        string FullSignature { get; }
        IVariableColumnSignature VariableColumnSignature { get; set; }
        List<int> NumbersOfAssotiatedBits { get; set; }

        bool IsBitOptionEqual(IBitOption comparingBitOption);
    }
}