using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Formatting.Editor.Services
{
  public  class FormatterInfoService: IFormatterInfoService
    {
        public bool ReturnsString(IUshortsFormatterViewModel formatterViewModel)
        {
            if (formatterViewModel is AsciiStringFormatterViewModel)
            {
                return true;
            }
            if (formatterViewModel is StringFormatter1251ViewModel)
            {
                return true;
            }
            return false;
        }
    }
}
