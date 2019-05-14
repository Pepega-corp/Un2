using System;
using System.Threading;
using System.Windows.Threading;

namespace Unicon2.FreezeNotifier
{
    public class FreezeNotifier : IDisposable
    {
        private readonly BlockDetector _blockDetector;

        private INotifyWindow _notifyWindow;

        private Dispatcher _threadDispacher;

        private readonly CreateWindowDelegate _createWindowDelegate;

        public FreezeNotifier(CreateWindowDelegate createWindowDelegate)
        {
            this._createWindowDelegate = createWindowDelegate;
            this._blockDetector = new BlockDetector();
            this._blockDetector.UIBlocked += this.ShowNotify;
            this._blockDetector.UIReleased += this.HideNotify;
        }

        ~FreezeNotifier()
        {
            this.Stop();
        }

        public void Start()
        {
            this._blockDetector.Start();
        }

        public void Stop()
        {
            this.Dispose();
        }

        private void ShowNotify()
        {
            Thread thread = new Thread((ThreadStart)delegate
           {
               this._threadDispacher = Dispatcher.CurrentDispatcher;
               SynchronizationContext.SetSynchronizationContext(
                   new DispatcherSynchronizationContext(this._threadDispacher));

               this._notifyWindow = this._createWindowDelegate.Invoke();
               this._notifyWindow.Closed += (sender, e) =>
                   this._threadDispacher.BeginInvokeShutdown(DispatcherPriority.Background);
               this._notifyWindow.Show();

               Dispatcher.Run();
           });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
        }

        private void HideNotify()
        {
            if (this._threadDispacher != null)
            {
                this._threadDispacher.BeginInvoke(new Action(() => this._notifyWindow.Close()));
            }
        }

        public void Dispose()
        {
            this._blockDetector.Stop();
            GC.SuppressFinalize(this);
        }

    }
}