using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels.Helpers
{
    public class BitOwnershipHelper
    {
       public static List<IBitViewModel> CreateBitViewModelsWithOwnership(IBitsConfigViewModel bitsConfigViewModel, List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            var result = new List<IBitViewModel>();

            
            for (int i = 15; i >= 0; i--)
            {
                var
                    bitOwnershipInfo = GetBitOwnerInfo(bitsConfigViewModel.GetAddressInfo().address,i,rootConfigurationItemViewModels);
                IBitViewModel bitViewModel = new BitViewModel(i, true);
                result.Add(bitViewModel);
            }


            return result;

        }

        private static (bool isAlreadySet, string ownerInfo) GetBitOwnerInfo(ushort address, int i, List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            throw new NotImplementedException();
        }
    }
}
