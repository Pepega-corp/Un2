
using System;

namespace Unicon2.Infrastructure.Interfaces.Resourcres
{
    public interface IItemReferenceResource : IResource
    {
        object ItemReferenced { get; set; }
        Guid ObjectGuid { get; set; }
        Action ItenInitializedAction { get; set; }
    }
}