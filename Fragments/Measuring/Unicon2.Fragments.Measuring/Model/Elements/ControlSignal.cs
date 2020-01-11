using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [DataContract(Namespace = "ControlSignalNS")]
    public class ControlSignal : MeasuringElementBase, IControlSignal
    {
        private IDataProvider _dataProvider;

        public ControlSignal(IWritingValueContext writingValueContext)
        {
            this.WritingValueContext = writingValueContext;
        }

        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL;

        [DataMember]
        public IWritingValueContext WritingValueContext { get; set; }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task<bool> Write()
        {
            IQueryResult queryResult = null;
            if (this.WritingValueContext.NumberOfFunction == 16)
            {
                queryResult = await this._dataProvider.WriteMultipleRegistersAsync(this.WritingValueContext.Address,
                    new[] { this.WritingValueContext.ValueToWrite }, "Write");

            }
            else if (this.WritingValueContext.NumberOfFunction == 5)
            {
                queryResult = await this._dataProvider.WriteSingleCoilAsync(this.WritingValueContext.Address,
                     this.WritingValueContext.ValueToWrite != 0, "Write");

            }
            if (queryResult != null)
            {
                return queryResult.IsSuccessful;
            }
            return false;
        }
    }
}
