using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Connections.MockConnection.ViewModels
{
   public class MockConnectionConfigurationViewModel:ViewModelBase,IViewModel
    {
        #region Implementation of IStronglyNamed

        public string StrongName { get; }

        #endregion

        #region Implementation of IViewModel

        public object Model { get; set; }

        #endregion
    }
}
