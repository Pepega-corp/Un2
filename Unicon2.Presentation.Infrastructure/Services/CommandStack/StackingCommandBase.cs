using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Presentation.Infrastructure.Services.CommandStack
{
    public abstract class StackingCommandBase:IStackingCommand
    {
        protected void AddDependencies(params ICommandStackDependencySource[] dependencySources)
        {
            Dependencies = new HashSet<ICommandStackDependencySource>(dependencySources);
        }

        public HashSet<ICommandStackDependencySource> Dependencies { get; private set; }
        public abstract string ViewName { get; }
        public abstract Task Do();

        public abstract Task Undo();
    }
}
