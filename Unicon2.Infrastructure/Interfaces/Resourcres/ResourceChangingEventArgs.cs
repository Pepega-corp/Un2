using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Interfaces.Resourcres
{
   public class ResourceChangingEventArgs
    {
        public string ParameterKey { get; set; }
        public object Value { get; set; }
    }
}
