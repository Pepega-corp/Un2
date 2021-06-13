using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Formatting.Editor.ViewModels.Helpers
{
    public class CodeStringHelper
    {
        public static string  GetStringBetweenCaretAndPrevSpecialChar(string fullStr, int caretPosition)
        {
            var startIndex = fullStr.Substring(0, caretPosition).LastIndexOfAny(new[] { '(', ')', ',' })+1;
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            return fullStr.Substring(startIndex, caretPosition-startIndex);
        }
    }
}
