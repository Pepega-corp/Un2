using System;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IUshortsFormatter : ICloneable, INameable
    {
        T Accept<T>(IFormatterVisitor<T> visitor);
    }
}