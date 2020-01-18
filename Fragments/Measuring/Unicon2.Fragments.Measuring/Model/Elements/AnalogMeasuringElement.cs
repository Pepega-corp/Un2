using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "AnalogMeasuringElementNS")]
    public class AnalogMeasuringElement : MeasuringElementBase, IAnalogMeasuringElement
    {
        private IDataProvider _dataProvider;

        public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task Load()
        {
            if (this.UshortsFormatter is ILoadable)
            {
                await (this.UshortsFormatter as ILoadable).Load();
            }
            IQueryResult<ushort[]> queryResult = await this._dataProvider.ReadHoldingResgistersAsync(this.Address, this.NumberOfPoints, "read");
            if (queryResult.IsSuccessful)
            {
                ushort[] ushorts = queryResult.Result;
                this.DeviceUshortsValue = ushorts;
                this.ElementChangedAction?.Invoke();
            }
        }

        public ushort[] DeviceUshortsValue { get; set; }
        [DataMember]
        public ushort Address { get; set; }
        [DataMember]
        public ushort NumberOfPoints { get; set; }

        [DataMember]
        public IUshortsFormatter UshortsFormatter { get; set; }

        [DataMember]
        public string MeasureUnit { get; set; }
        [DataMember]
        public bool IsMeasureUnitEnabled { get; set; }

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;
            (this.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(container);
            this.IsInitialized = true;
        }
    }
}
