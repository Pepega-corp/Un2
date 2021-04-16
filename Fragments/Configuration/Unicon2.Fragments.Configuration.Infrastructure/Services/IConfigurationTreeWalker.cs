using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Fragments.Configuration.Infrastructure.Services
{
    public interface IConfigurationTreeWalker
    {
        bool IsStructureSimilar(IConfigurationItemViewModel configurationItemViewModel1,
            IConfigurationItemViewModel configurationItemViewModel2);

        void CopyValuesToItem(IConfigurationItemViewModel configurationItemViewModelFrom,
            IConfigurationItemViewModel configurationItemViewModelTo);
    }
}