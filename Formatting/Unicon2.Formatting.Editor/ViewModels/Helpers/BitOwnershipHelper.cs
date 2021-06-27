using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Tree;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Properties;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels.Helpers
{
    public class BitOwnershipHelper
    {
       public static List<IBitViewModel> CreateBitViewModelsWithOwnership(IBitsConfigViewModel bitsConfigViewModel, List<IConfigurationItemViewModel> rootConfigurationItemViewModels)
        {
            var result = new List<IBitViewModel>();

            List<IConfigurationItemViewModel> itemsWithSameAddress=new List<IConfigurationItemViewModel>();
            FillItemsWithSameAddress(bitsConfigViewModel.GetAddressInfo().address,rootConfigurationItemViewModels,itemsWithSameAddress);
            if (itemsWithSameAddress.Contains(bitsConfigViewModel as IConfigurationItemViewModel))
            {
                itemsWithSameAddress.Remove(bitsConfigViewModel as IConfigurationItemViewModel);
            }

            for (int i = 15; i >= 0; i--)
            {
                IEnumerable<(bool isBitOwned ,string owner)> owners= itemsWithSameAddress.Select(model =>
                {
                    if (model is IBitsConfigViewModel bitsConfigViewModelSameAddress &&
                        bitsConfigViewModelSameAddress.IsFromBits)
                    {
                        var bit= bitsConfigViewModelSameAddress.BitNumbersInWord.FirstOrDefault(viewModel =>
                            viewModel.BitNumber == i && viewModel.IsChecked);
                        if (bit != null)
                        {
                            return (true, GetMessageForAlreadySetItem(model));
                        }
                        else
                        {
                            return (false, string.Empty);
                        }
                    }

                    return (true, GetMessageForAlreadySetItem(model));

                }).ToList();

                var isOwned = owners.Any(tuple => tuple.isBitOwned);
                var toolTip = owners.FirstOrDefault(tuple => tuple.isBitOwned).owner;


                IBitViewModel bitViewModel = new BitViewModel(i, !isOwned,toolTip);
                result.Add(bitViewModel);
            }


            return result;

        }


       private static string GetMessageForAlreadySetItem(IConfigurationItemViewModel configurationItemViewModel)
       {
           return
               $"{StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString("ThisBitSetIn")} {Environment.NewLine} {configurationItemViewModel.BuildItemPath()}";
       }

        private static void FillItemsWithSameAddress(ushort address, List<IConfigurationItemViewModel> initialListToSearch, List<IConfigurationItemViewModel> accumulator)
        {
            foreach (var configurationItemViewModel in initialListToSearch)
            {
                if (configurationItemViewModel is IWithAddressViewModel withAddress)
                {
                    if (withAddress.Address == address.ToString())
                    {
                        accumulator.Add(configurationItemViewModel);
                    }
                }

                if (configurationItemViewModel.ChildStructItemViewModels.Any())
                {
                    FillItemsWithSameAddress(address, configurationItemViewModel.ChildStructItemViewModels.ToList(),accumulator);
                }
            }
        }
    }
}
