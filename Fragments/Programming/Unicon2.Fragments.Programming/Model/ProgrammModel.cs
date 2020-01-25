using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Namespace = "ProgrammModelNS")]
    public class ProgrammModel : IProgrammModel
    {
        private IDataProvider _dataProvider;

        public ProgrammModel()
        {
            this.Elements = new List<ILogicElement>();
        }

        [DataMember]
        public List<ILogicElement> Elements { get; set; }

        #region Implementation of IStronglyNamed

        public string StrongName => ProgrammingKeys.PROGRAMMING;

        #endregion

        #region Implementation of IDeviceFragment
        [DataMember]
        public IFragmentSettings FragmentSettings { get; set; }
        #endregion

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }
    }
}
