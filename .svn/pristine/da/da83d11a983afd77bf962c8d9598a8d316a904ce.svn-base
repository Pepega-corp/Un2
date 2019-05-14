using System;
using NModbus4.IO;
using NModbus4.Message;
using Xunit;

namespace NModbus4.UnitTests.IO
{
    public static class EmptyTransportFixture
    {
        [Fact]
        public static void Negative()
        {
            var transport = new EmptyTransport();
            Assert.Throws<NotImplementedException>(() => transport.ReadRequest());
            Assert.Throws<NotImplementedException>(() => transport.ReadResponse<ReadCoilsInputsResponse>());
            Assert.Throws<NotImplementedException>(() => transport.BuildMessageFrame(null));
            Assert.Throws<NotImplementedException>(() => transport.Write(null));
            Assert.Throws<NotImplementedException>(() => transport.OnValidateResponse(null, null));
        }
    }
}
