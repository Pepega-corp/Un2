using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;

namespace Unicon2.Fragments.Configuration.Model.Dependencies.Results
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HidePropertyResult:IHidePropertyResult
    {
    }
}
