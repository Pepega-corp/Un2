using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeFormatter : UshortsFormatterBase, ICodeFormatter
    {
        private CodeFormatterExpression _codeExpression;

        public override object Clone()
        {
            return new CodeFormatter()
            {
                FormatBackCodeString = FormatBackCodeString,
                FormatCodeString = FormatCodeString
            };
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitCodeFormatter(this);
        }

        [JsonProperty] public string FormatCodeString { get; set; }
        [JsonProperty] public string FormatBackCodeString { get; set; }

        public CodeFormatterExpression CodeExpression
        {
            get
            {
                if (_codeExpression == null)
                {
                    _codeExpression=new CodeFormatterExpression(FormatCodeString,FormatBackCodeString);
                }
                return _codeExpression;

            }
        }
    }
}
