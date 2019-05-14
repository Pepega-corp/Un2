using System;
using System.Threading;
using System.Windows.Threading;

namespace Unicon2.FreezeNotifier
{
    internal class BlockDetector
    {
        private bool _isBusy;

        private const int FreezeTimeLimit = 1000;

        private readonly DispatcherTimer _foregroundTimer;

        private readonly Timer _backgroundTimer;

        private DateTime _lastForegroundTimerTickTime;

        public event Action UIBlocked;

        public event Action UIReleased;

        public BlockDetector()
        {
            this._foregroundTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(FreezeTimeLimit / 2) };
            this._foregroundTimer.Tick += this.ForegroundTimerTick;

            this._backgroundTimer = new Timer(this.BackdroundTimerTick, null, FreezeTimeLimit, Timeout.Infinite);
        }

        private void BackdroundTimerTick(object someObject)
        {
            double totalMilliseconds = (DateTime.Now - this._lastForegroundTimerTickTime).TotalMilliseconds;
            if (totalMilliseconds > FreezeTimeLimit && this._isBusy == false)
            {
                this._isBusy = true;
                Dispatcher.CurrentDispatcher.Invoke(() => UIBlocked()); ;
            }
            else
            {
                if (totalMilliseconds < FreezeTimeLimit && this._isBusy)
                {
                    this._isBusy = false;
                    Dispatcher.CurrentDispatcher.Invoke(() => UIReleased()); ;
                }

            }
            this._backgroundTimer.Change(FreezeTimeLimit, Timeout.Infinite);
        }

        private void ForegroundTimerTick(object sender, EventArgs e)
        {
            this._lastForegroundTimerTickTime = DateTime.Now;
        }

        public void Start()
        {
            this._foregroundTimer.Start();
        }

        public void Stop()
        {
            this._foregroundTimer.Stop();
            this._backgroundTimer.Dispose();
        }
    }
}
