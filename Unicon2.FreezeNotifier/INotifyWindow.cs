using System;

namespace Unicon2.FreezeNotifier
{
    public interface INotifyWindow
    {
        event EventHandler Closed;
        void Show();
        void Close();
    }


}
