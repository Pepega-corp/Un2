namespace Unicon2.Unity.Common
{
    public class ResolverParameter
    {
        public string ParameterName { get; }
        public object ParameterValue { get; }

        public ResolverParameter(string name, object value)
        {
            this.ParameterName = name;
            this.ParameterValue = value;
        }
    }
}
