using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Formatting.Infrastructure.ViewModel
{
    public interface ICodeFormatterViewModel : IDynamicFormatterViewModel
    {
        string FormatCodeString { get; set; }
        string FormatBackCodeString { get; set; }
    }
}