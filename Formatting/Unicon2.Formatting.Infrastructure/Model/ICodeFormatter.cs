using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface ICodeFormatter : IUshortsFormatter
    {
        string FormatCodeString { get; set; }
        string FormatBackCodeString { get; set; }
        CodeFormatterExpression CodeExpression { get; }
    }
}
