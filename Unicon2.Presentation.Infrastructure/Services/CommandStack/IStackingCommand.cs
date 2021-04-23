using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unicon2.Presentation.Infrastructure.Services.CommandStack
{
    public interface IStackingCommand
    {
        HashSet<ICommandStackDependencySource> Dependencies { get; }
        string ViewName { get; }
        Task Do();
        Task Undo();
    }
}