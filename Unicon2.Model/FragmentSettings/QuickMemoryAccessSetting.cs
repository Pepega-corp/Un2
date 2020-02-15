using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    [DataContract(Namespace = "QuickMemoryAccessSettingNS")]
    public class QuickMemoryAccessSetting : IQuickMemoryAccessSetting
    {
        private IDataProvider _dataProvider;
        private bool _isInitialized;

        public QuickMemoryAccessSetting()
        {
            this.QuickAccessAddressRanges = new List<IRange>();
        }


        public string StrongName => ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING;

        [DataMember]
        public bool IsSettingEnabled { get; set; }
        public async Task<bool> ApplySetting(ISettingApplyingContext settingApplyingContext)
        {
            this._quickAccessMemoryApplyingContext = settingApplyingContext as IQuickAccessMemoryApplyingContext;
            List<IRange> ranges = new List<IRange>();
            this._quickAccessMemoryApplyingContext.DataProviderContainingObjectList.ForEach((containing =>
            {
                List<IRange> rangesToAdd = containing.GetAddressesRanges();
                foreach (IRange rangeToAdd in rangesToAdd)
                {
                    if (!ranges.Any((range => range.CheckNesting(rangeToAdd))))
                    {
                        ranges.Add(rangeToAdd);
                    }
                }

            }));

            if (this._quickAccessMemoryApplyingContext.QuickAccessMode == QuickAccessModeEnum.Initialize)
            {
                this.QuickMemoryAccessDataProviderStub = new QuickMemoryAccessDataProviderStub(StaticContainer.Container.Resolve<IQueryResultFactory>());
                this.QuickMemoryAccessDataProviderStub.SetDataProvider(this._dataProvider);
                foreach (IRange range in this.QuickAccessAddressRanges)
                {
                    if (ranges.Any((rangeToCheck => range.CheckNesting(rangeToCheck))))
                    {
                        IQueryResult<ushort[]> queryResult = await this._dataProvider.ReadHoldingResgistersAsync((ushort)range.RangeFrom,
                             (ushort)(range.RangeTo - range.RangeFrom + 1), this._quickAccessMemoryApplyingContext.QueryNameKey).ConfigureAwait(false);

                        this.QuickMemoryAccessDataProviderStub.MemoryValuesSets.Add(new MemoryValueSet(range, queryResult.Result));
                    }
                }
                this._quickAccessMemoryApplyingContext.DataProviderContainingObjectList.ForEach((item => item.SetDataProvider(this.QuickMemoryAccessDataProviderStub)));
            }
            if (this._quickAccessMemoryApplyingContext.QuickAccessMode == QuickAccessModeEnum.Write)
            {
                int index = 0;
                foreach (IRange range in this.QuickAccessAddressRanges)
                {
                    if (this.QuickMemoryAccessDataProviderStub.MemoryValuesSets.Count <= index) continue;
                    ushort[] ushortsValue = this.QuickMemoryAccessDataProviderStub.MemoryValuesSets[index++]
                        .AddressesValuesDictionary.Values.ToArray();
                    IQueryResult queryResult = await this._dataProvider.WriteMultipleRegistersAsync((ushort)range.RangeFrom, ushortsValue, this._quickAccessMemoryApplyingContext.QueryNameKey);
                    if (!queryResult.IsSuccessful) return false;
                }
            }
            return this.IsSettingEnabled;
        }

        [DataMember]
        public List<IRange> QuickAccessAddressRanges { get; set; }
        public IQuickMemoryAccessDataProviderStub QuickMemoryAccessDataProviderStub { get; set; }
        
    }
}
