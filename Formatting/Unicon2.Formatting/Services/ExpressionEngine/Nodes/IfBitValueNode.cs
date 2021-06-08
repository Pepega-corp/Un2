namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class IfBitValueNode : RuleNodeBase
    {
        private readonly string _resourceName;
        private readonly ushort _bitNum;

        public IfBitValueNode(string resourceName, ushort bitNum)
        {
            _resourceName = resourceName;
            _bitNum = bitNum;
        }


    }
}