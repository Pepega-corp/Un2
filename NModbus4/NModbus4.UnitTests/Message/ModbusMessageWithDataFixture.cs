using NModbus4.Data;
using NModbus4.Message;
using Xunit;

namespace NModbus4.UnitTests.Message
{
    public class ModbusMessageWithDataFixture
    {
        [Fact]
        public void ModbusMessageWithDataFixtureCtorInitializesProperties()
        {
            AbstractModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(Modbus.ReadCoils, 10, 1,
                new DiscreteCollection(true, false, true));
            Assert.Equal(Modbus.ReadCoils, message.FunctionCode);
            Assert.Equal(10, message.SlaveAddress);
        }

        [Fact]
        public void ProtocolDataUnitReadCoilsResponse()
        {
            AbstractModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(Modbus.ReadCoils, 1, 2,
                new DiscreteCollection(true));
            byte[] expectedResult = { 1, 2, 1 };
            Assert.Equal<byte[]>(expectedResult, message.ProtocolDataUnit);
        }

        [Fact]
        public void DataReadCoilsResponse()
        {
            DiscreteCollection col = new DiscreteCollection(false, true, false, true, false, true, false, false, false,
                false);
            AbstractModbusMessageWithData<DiscreteCollection> message = new ReadCoilsInputsResponse(Modbus.ReadCoils, 11, 1, col);
            Assert.Equal(col.Count, message.Data.Count);
            Assert.Equal(col.NetworkBytes, message.Data.NetworkBytes);
        }
    }
}