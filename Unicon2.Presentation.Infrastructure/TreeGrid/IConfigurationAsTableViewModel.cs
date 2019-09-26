using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Presentation.Infrastructure.TreeGrid
{
   public interface IConfigurationAsTableViewModel
    {
        bool IsTableView { get; set; }
        IConfigurationItemViewModel TableConfigurationViewModel { get; set; }
    }
}
