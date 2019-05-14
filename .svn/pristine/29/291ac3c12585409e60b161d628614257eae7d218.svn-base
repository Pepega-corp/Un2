using System;
using NModbus4.Message;
using Xunit;

namespace NModbus4.UnitTests.Message
{
    public class ModbusMessageImplFixture
    {
        [Fact]
        public void ModbusMessageCtorInitializesProperties()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl(5, Modbus.ReadCoils);
            Assert.Equal(5, messageImpl.SlaveAddress);
            Assert.Equal(Modbus.ReadCoils, messageImpl.FunctionCode);
        }

        [Fact]
        public void Initialize()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl();
            messageImpl.Initialize(new byte[] { 1, 2, 9, 9, 9, 9 });
            Assert.Equal(1, messageImpl.SlaveAddress);
            Assert.Equal(2, messageImpl.FunctionCode);
        }

        [Fact]
        public void ChecckInitializeFrameNull()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl();
            Assert.Throws<ArgumentNullException>(() => messageImpl.Initialize(null));
        }

        [Fact]
        public void InitializeInvalidFrame()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl();
            Assert.Throws<FormatException>(() => messageImpl.Initialize(new byte[] { 1 }));
        }

        [Fact]
        public void ProtocolDataUnit()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl(11, Modbus.ReadCoils);
            byte[] expectedResult = { Modbus.ReadCoils };
            Assert.Equal<byte[]>(expectedResult, messageImpl.ProtocolDataUnit);
        }

        [Fact]
        public void MessageFrame()
        {
            ModbusMessageImpl messageImpl = new ModbusMessageImpl(11, Modbus.ReadHoldingRegisters);
            byte[] expectedMessageFrame = { 11, Modbus.ReadHoldingRegisters };
            Assert.Equal<byte[]>(expectedMessageFrame, messageImpl.MessageFrame);
        }
    }
}