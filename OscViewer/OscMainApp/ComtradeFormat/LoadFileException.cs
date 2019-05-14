using System;

namespace Oscilloscope.ComtradeFormat
{
  public  class LoadFileException:Exception
    {
      public LoadFileException(string message)
          : base(message)
        { }
    }
}
