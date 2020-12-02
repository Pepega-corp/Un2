using System.Collections.Generic;

namespace Unicon2.Tests.Helpers.Query
{
    public class QueryMockDefinition
    {
        public QueryMockDefinition(ushort address, ushort funcNumber, ushort? numberOfPoints = null,
            List<ushort> data = null)
        {
            Address = address;
            NumberOfPoints = numberOfPoints;
            FuncNumber = funcNumber;
            Data = data;
        }

        public ushort Address { get; }
        public ushort? NumberOfPoints { get; }
        public ushort FuncNumber { get; }
        public List<ushort> Data { get; }

    }
}