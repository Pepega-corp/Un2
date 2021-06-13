using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;

namespace Unicon2.Formatting.Services.ExpressionEngine.Visitors
{
    public class DoVisitor : LexemaVisitor
    {
        public override IRuleNode VisitStringPart(string str)
        {
            var get = str.GetParameterFromString();
            return new DoNode(get.Select(s => LexemManager.GetNodeByString(s)));
        }

        public DoVisitor(LexemManager lexemManager) : base(lexemManager)
        {

        }
    }
}
