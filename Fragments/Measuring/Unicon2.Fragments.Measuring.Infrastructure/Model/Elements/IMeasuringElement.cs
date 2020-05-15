using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IMeasuringElement : INameable, IStronglyNamed, IUniqueIdWithSet
    {
        List<IDependency> Dependencies { get; set; }
    }
}