using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "DescretMeasuringElementNS")]
    public class DescretMeasuringElement : MeasuringElementBase, IDiscretMeasuringElement
    {
        private IDataProvider _dataProvider;
        private bool _isLoadingInProcess;
        private Action _stopReadingAction;

        public override string StrongName => MeasuringKeys.DISCRET_MEASURING_ELEMENT;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }


        public async Task Load()
        {
            if ((this.AddressOfBit.NumberOfFunction == 3) || (this.AddressOfBit.NumberOfFunction == 4))
            {
                IQueryResult<ushort[]> queryResult =
                    await this._dataProvider.ReadHoldingResgistersAsync(this.AddressOfBit.Address, 1, "Read");
                if (queryResult.IsSuccessful)
                {
                    if ((this.AddressOfBit.NumberOfFunction == 3) || (this.AddressOfBit.NumberOfFunction == 4))
                    {
                        BitArray bitarr = new BitArray(new[] { (int)(queryResult.Result)[0] });
                        bool bitResult = bitarr[(this.AddressOfBit).BitAddressInWord];
                        if (bitResult == this.DeviceValue) return;
                        this.DeviceValue = bitResult;
                        this.ElementChangedAction?.Invoke();
                    }

                }
            }
            else if ((this.AddressOfBit.NumberOfFunction == 1) || (this.AddressOfBit.NumberOfFunction == 2))
            {
                IQueryResult<bool> boolQueryResult =
                    await this._dataProvider.ReadCoilStatusAsync(this.AddressOfBit.Address, MeasuringKeys.READ_SINGLE_BIT_QUERY);
                if (boolQueryResult.IsSuccessful)
                {

                    if (boolQueryResult.Result == this.DeviceValue) return;
                    this.DeviceValue = boolQueryResult.Result;
                    this.ElementChangedAction?.Invoke();
                }
            }

        }


        [DataMember]
        public IAddressOfBit AddressOfBit { get; set; }

        public bool DeviceValue { get; set; }

        public ushort Address => this.AddressOfBit.Address;

        public ushort NumberOfPoints => 1;
    }
}
