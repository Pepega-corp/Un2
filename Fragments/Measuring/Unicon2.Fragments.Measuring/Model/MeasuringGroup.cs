using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Model
{
    [DataContract(Namespace = "MeasuringGroupNS")]
    public class MeasuringGroup : IMeasuringGroup
    {
        public MeasuringGroup(Func<IRange> rangesGettingFunc)
        {
            this.MeasuringElements = new List<IMeasuringElement>();
        }
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<IMeasuringElement> MeasuringElements { get; set; }

 
    }
}
