namespace Unicon2.Tests.Helpers.Query
{
    public class QueryMockDefinition
    {
        public QueryMockDefinition(ushort address, ushort funcNumber, ushort? numberOfPoints = null,
            ushort[] data = null)
        {
            Address = address;
            NumberOfPoints = numberOfPoints;
            FuncNumber = funcNumber;
            Data = data;
        }

        public ushort Address { get; }
        public ushort? NumberOfPoints { get; }
        public ushort FuncNumber { get; }
        public ushort[] Data { get; }

    }
}