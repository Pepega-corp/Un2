using System.Runtime.Serialization;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract]
    public class ProgrammModel : IProgrammModel
    {
        private IDataProvider _dataProvider;

        //public ProgrammModel()
        //{
            
        //}

        #region Implementation of IStronglyNamed

        public string StrongName => ProgrammingKeys.PROGRAMMING;

        #endregion

        #region Implementation of IDeviceFragment
        
        public IFragmentSettings FragmentSettings { get; set; }

        #endregion

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }
    }
}
