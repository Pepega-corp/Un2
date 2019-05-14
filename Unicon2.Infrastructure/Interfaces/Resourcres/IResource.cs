using System;
using System.Security.Policy;

namespace Unicon2.Infrastructure.Interfaces.Resourcres
{
    public interface IResource:IDisposable,ICloneable,INameable,IStronglyNamed
    {
        bool IsAddedGlobal { get; set; }
    }
}