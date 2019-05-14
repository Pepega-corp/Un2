using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.Model
{
    public interface IElementLibraryModel : IStronglyNamed
    {
        List<ILogicElement> Elements { get; }
    }
}