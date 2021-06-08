namespace Unicon2.Formatting.Services.ExpressionEngine.Nodes
{
    public class SetBitValueNode : RuleNodeBase
    {
        private readonly string _resourceName;
        private readonly ushort _bitNum;
        private readonly bool _bitValue;

        public SetBitValueNode(string resourceName, ushort bitNum, bool bitValue)
        {
            _resourceName = resourceName;
            _bitNum = bitNum;
            _bitValue = bitValue;
        }


    }
}