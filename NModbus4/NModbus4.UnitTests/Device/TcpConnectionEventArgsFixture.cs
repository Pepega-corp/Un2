using System;
using NModbus4.Device;
using Xunit;

namespace NModbus4.UnitTests.Device
{
    public class TcpConnectionEventArgsFixture
    {
        [Fact]
        public void TcpConnectionEventArgs_NullEndPoint()
        {
            Assert.Throws<ArgumentNullException>(() => new TcpConnectionEventArgs(null));
        }

        [Fact]
        public void TcpConnectionEventArgs_EmptyEndPoint()
        {
            Assert.Throws<ArgumentException>(() => new TcpConnectionEventArgs(string.Empty));
        }

        [Fact]
        public void TcpConnectionEventArgs()
        {
            var args = new TcpConnectionEventArgs("foo");

            Assert.Equal((string) "foo", (string) args.EndPoint);
        }
    }
}