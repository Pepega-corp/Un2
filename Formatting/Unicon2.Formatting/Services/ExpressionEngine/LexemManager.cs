using System.Collections.Generic;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;
using Unicon2.Formatting.Services.ExpressionEngine.Visitors;

namespace Unicon2.Formatting.Services.ExpressionEngine
{
    public class LexemManager
    {
        public LexemManager()
        {
            KnownLexemaVisitors.Add("Add", new AddVisitor(this));
            KnownLexemaVisitors.Add("GetDeviceValue", new GetDeviceValueVisitor(this));
            KnownLexemaVisitors.Add("SetDeviceValue", new SetDeviceValueVisitor(this));
            KnownLexemaVisitors.Add("SetResultValue", new SetResultValueVisitor(this));
            KnownLexemaVisitors.Add("GetResultValue", new GetResultValueVisitor(this));
            KnownLexemaVisitors.Add("Pow", new GetDeviceValueVisitor(this));
            KnownLexemaVisitors.Add("Multiply", new MultiplyVisitor(this));
            KnownLexemaVisitors.Add("Subtract", new SubtractVisitor(this));
            KnownLexemaVisitors.Add("Divide", new DivideVisitor(this));
            KnownLexemaVisitors.Add("Compare", new CompareVisitor(this));
            KnownLexemaVisitors.Add("GetBitOfResource", new GetBitOfResourceVisitor(this));
            KnownLexemaVisitors.Add("SetBitOfResource", new SetBitOfResourceVisitor(this));

            KnownLexemaVisitors.Add("SetVariable", new SetBitOfResourceVisitor(this));
            KnownLexemaVisitors.Add("GetVariable", new SetBitOfResourceVisitor(this));

            KnownLexemaVisitors.Add("If", new IfVisitor(this));
        }

        public IRuleNode GetNodeByString(string str)
        {
            if (double.TryParse(str, out var result))
            {
                return new NumberNode(result);
            }

            var name = str.Substring(0, str.IndexOf('('));
            var parametersString = str.Substring(str.IndexOf('(') + 1, str.LastIndexOf(')') - str.IndexOf('(') - 1);

            return KnownLexemaVisitors[name].VisitStringPart(parametersString);
        }

        public Dictionary<string, LexemaVisitor> KnownLexemaVisitors { get; } =
            new Dictionary<string, LexemaVisitor>();

    }
}