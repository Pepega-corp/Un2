using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NModbus4.Data;
using NModbus4.UnitTests.Message;
using Xunit;
using NModbus4.Unme.Common;

namespace NModbus4.UnitTests.Utility
{
    public class CollectionUtilityFixture
    {
        [Fact]
        public void SliceMiddle()
        {
            byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.Equal(new byte[] { 3, 4, 5, 6, 7 }, Enumerable.ToArray<byte>(test.Slice(2, 5)));
        }

        [Fact]
        public void SliceBeginning()
        {
            byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.Equal(new byte[] { 1, 2 }, Enumerable.ToArray<byte>(test.Slice(0, 2)));
        }

        [Fact]
        public void SliceEnd()
        {
            byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Assert.Equal(new byte[] { 9, 10 }, Enumerable.ToArray<byte>(test.Slice(8, 2)));
        }

        [Fact]
        public void SliceCollection()
        {
            Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
            Assert.Equal(new bool[] { false, false, true }, Enumerable.ToArray<bool>(col.Slice(2, 3)));
        }

        [Fact]
        public void SliceReadOnlyCollection()
        {
            ReadOnlyCollection<bool> col =
                new ReadOnlyCollection<bool>(new bool[] { true, false, false, false, true, true });
            Assert.Equal(new bool[] { false, false, true }, Enumerable.ToArray<bool>(col.Slice(2, 3)));
        }

        [Fact]
        public void SliceNullICollection()
        {
            ICollection<bool> col = null;
            Assert.Throws<ArgumentNullException>(() => Enumerable.ToArray<bool>(col.Slice(1, 1)));
        }

        [Fact]
        public void SliceNullArray()
        {
            bool[] array = null;
            Assert.Throws<ArgumentNullException>(() => Enumerable.ToArray<bool>(array.Slice(1, 1)));
        }

        [Fact]
        public void CreateDefaultCollectionNegativeSize()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MessageUtility.CreateDefaultCollection<RegisterCollection, ushort>(0, -1));
        }

        [Fact]
        public void CreateDefaultCollection()
        {
            RegisterCollection col = MessageUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, 5);
            Assert.Equal(5, col.Count);
            Assert.Equal(new ushort[] { 3, 3, 3, 3, 3 }, Enumerable.ToArray<ushort>(col));
        }
    }
}