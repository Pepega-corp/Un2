using NModbus4.Data;
using NModbus4.Message;
using Xunit;

namespace NModbus4.UnitTests.Message
{
    public class DiagnosticsRequestResponseFixture
    {
        [Fact]
        public void ToString_Test()
        {
            DiagnosticsRequestResponse response;

            response = new DiagnosticsRequestResponse(Modbus.DiagnosticsReturnQueryData, 3, new RegisterCollection(5));
            Assert.Equal((string) "Diagnostics message, sub-function return query data - {5}.", (string) response.ToString());
        }
    }
}