using System.Linq;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class AddVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new AddNode(get.Select(s =>LexemManager.GetNodeByString(s)).ToList());
        }

        public AddVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}