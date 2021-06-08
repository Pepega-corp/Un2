using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class GetDeviceValueVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new GetDeviceValueNode(LexemManager.GetNodeByString(get[0]));
        }


        public GetDeviceValueVisitor(LexemManager lexemManager) : base(lexemManager)
        {
        }
    }
}