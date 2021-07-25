using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unicon2.Formatting.Services.ExpressionEngine.Common;
using Unicon2.Formatting.Services.ExpressionEngine.Nodes;
using Unicon2.Formatting.Services.ExpressionEngine.Visitors;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Formatting.Services.ExpressionEngine
{
    public class LexemManager
    {
        private readonly ILocalizerService _localizerService;

        public LexemManager(ILocalizerService localizerService)
        {
            _localizerService = localizerService;

            AddVisitorInfo("Add", new AddVisitor(this));
            AddVisitorInfo("GetDeviceValue", new GetDeviceValueVisitor(this));
            AddVisitorInfo("SetDeviceValue", new SetDeviceValueVisitor(this));
            AddVisitorInfo("SetResultValue", new SetResultValueVisitor(this));
            AddVisitorInfo("GetResultValue", new GetResultValueVisitor(this));
            AddVisitorInfo("Pow", new PowVisitor(this));
            AddVisitorInfo("Multiply", new MultiplyVisitor(this));
            AddVisitorInfo("Subtract", new SubtractVisitor(this));
            AddVisitorInfo("Divide", new DivideVisitor(this));
            AddVisitorInfo("Compare", new CompareVisitor(this));
            AddVisitorInfo("GetBitOfResource", new GetBitOfResourceVisitor(this));
            AddVisitorInfo("SetBitOfResource", new SetBitOfResourceVisitor(this));

            AddVisitorInfo("SetVariable", new SetVariableVisitor(this));
            AddVisitorInfo("GetVariable", new GetVariableVisitor(this));

            AddVisitorInfo("Select", new SelectVisitor(this));

            AddVisitorInfo("GetInputValue", new GetInputVisitor(this));

            AddVisitorInfo("If", (new IfVisitor(this)));
            AddVisitorInfo("Do", (new DoVisitor(this)));
            AddVisitorInfo("InvertSign", (new InvertSignVisitor(this)));


            AddVisitorInfo("ConvertUnsignedToInt16", (new ConvertUnsignedToInt16Visitor(this)));
            AddVisitorInfo("ConvertInt16ToUnsigned", (new ConvertInt16ToUnsignedVisitor(this)));
            AddVisitorInfo("HideCurrentProperty", (new HideCurrentPropertyVisitor(this)));

        }

        private void AddVisitorInfo(string name,LexemaVisitor lexemaVisitor)
        {
            KnownLexemaVisitors.Add(name,(lexemaVisitor,_localizerService.GetLocalizedString($"{name}FunctionDescription")));
        }

        public IRuleNode GetNodeByString(string str)
        {
            if (double.TryParse(str, out var result))
            {
                return new NumberNode(result);
            }
            if (double.TryParse(str, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out var result1))
            {
                return new NumberNode(result1);
            }

            if (bool.TryParse(str, out var boolResult))
            {
                return new BoolNode(boolResult);
            }

            var name = str.Substring(0, str.IndexOf('('));
            var parametersString = str.Substring(str.IndexOf('(') + 1, str.LastIndexOf(')') - str.IndexOf('(') - 1);
            
            try
            {
               var r= KnownLexemaVisitors[name].visitor.VisitStringPart(parametersString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return KnownLexemaVisitors[name].visitor.VisitStringPart(parametersString);
        }

        public Dictionary<string, (LexemaVisitor visitor, string description)> KnownLexemaVisitors { get; } =
            new Dictionary<string, (LexemaVisitor visitor, string description)>();

    }
}