using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Unicon2.Model.Values.Range;

namespace Unicon2.Tests.Configuration
{
    [TestFixture]
    public class RangeTests
    {
        [Test]
        public void CheckRangeNotStrongCompare()
        {
            DefaultRange defaultRange=new DefaultRange()
            {
                RangeFrom = 100,
                RangeTo = 200
            };
            Assert.True(defaultRange.CheckValue(200));
            Assert.False(defaultRange.CheckValue(201));
            Assert.True(defaultRange.CheckValue(100));
            Assert.True(defaultRange.CheckValue(101));
            Assert.False(defaultRange.CheckValue(99));
            Assert.True(defaultRange.CheckNesting(new DefaultRange()
            {
                RangeFrom = 100,
                RangeTo = 200
            }));
            Assert.True(defaultRange.CheckNesting(new DefaultRange()
            {
                RangeFrom = 101,
                RangeTo = 199
            }));
            Assert.False(defaultRange.CheckNesting(new DefaultRange()
            {
                RangeFrom = 99,
                RangeTo = 200
            }));
            Assert.False(defaultRange.CheckNesting(new DefaultRange()
            {
                RangeFrom = 100,
                RangeTo = 201
            }));
        }
    }
}